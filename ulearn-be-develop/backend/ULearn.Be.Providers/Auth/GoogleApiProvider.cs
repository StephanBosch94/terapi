using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.Auth;
using ULearn.Be.Providers.Auth.Interfaces;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.External;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.Providers.Auth;

public class GoogleApiProvider : IGoogleApiProvider
{
    private readonly GoogleApiSettings _googleApiSettings;
    private readonly ILogger<GoogleApiProvider> _logger;

    public GoogleApiProvider(IEnvConfigurationProvider envConfigurationProvider, ILogger<GoogleApiProvider> logger)
    {
        _logger = logger;
        _googleApiSettings =
            envConfigurationProvider.GetSettings<GoogleApiSettings>(EnvSettingsKeyEnum.GoogleApiSettings);
    }

    public async Task<AuthUserFromExternalSourceVM> ValidateToken(string token)
    {
        var result = await GoogleJsonWebSignature.ValidateAsync(token);
        return new AuthUserFromExternalSourceVM
        {
            Email = result.Email,
            Name = result.Name,
            FirstName = result.GivenName,
            LastName = result.FamilyName,
            Locale = result.Locale,
            PictureUrl = result.Picture,
            VerifiedEmail = result.EmailVerified,
        };
    }

    public async Task<AuthUserFromExternalSourceVM> Authenticate(AuthExternalLoginIM input)
    {
        var tokens = await GetTokens(input.AuthenticationCode, input.RedirectUrl);
        var userInfo = await GetUserInfo(tokens.AccessToken);
        return new AuthUserFromExternalSourceVM
        {
            Email = userInfo.Email,
            Name = userInfo.Name,
            FirstName = userInfo.GivenName,
            LastName = userInfo.FamilyName,
            Locale = userInfo.Locale,
            PictureUrl = userInfo.Picture,
            VerifiedEmail = userInfo.VerifiedEmail,
            Phone = null,
            Tokens = new List<ExternalUserTokenVM>
            {
                new()
                {
                    CTokenProviderEnum = CTokenProviderEnum.GoogleApi,
                    CTokenTypeEnum = CTokenTypeEnum.RefreshToken,
                    TokenValue = tokens.RefreshToken
                },
                new()
                {
                    CTokenProviderEnum = CTokenProviderEnum.GoogleApi,
                    CTokenTypeEnum = CTokenTypeEnum.IdToken,
                    TokenValue = tokens.IdToken
                },
                new()
                {
                    CTokenProviderEnum = CTokenProviderEnum.GoogleApi,
                    CTokenTypeEnum = CTokenTypeEnum.AccessToken,
                    TokenValue = tokens.AccessToken
                }
            }
        };
    }

    private async Task<GoogleAuthResultVM> GetTokens(string authCode, string inputRedirectUrl)
    {
        _logger.LogInformation("Calling Google API getTokens()");
        var httpClient = new HttpClient();
        var content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "code", authCode },
                { "client_id", _googleApiSettings.ClientId },
                { "client_secret", _googleApiSettings.ClientSecret },
                { "redirect_uri", inputRedirectUrl },
                { "grant_type", "authorization_code" }
            }
        );

        var tokensResponse = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);
        try
        {
            tokensResponse.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw new ForbiddenException();
        }

        var tokens =
            JsonConvert.DeserializeObject<GoogleAuthResultVM>(await tokensResponse.Content.ReadAsStringAsync());
        _logger.LogInformation("Calling Google API OK()");
        return tokens;
    }

    private async Task<GoogleUserDataVM> GetUserInfo(string accessToken)
    {
        _logger.LogInformation("Calling Google API getUserInfo()");
        var httpClient = new HttpClient();
        var userInfoRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(
                $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}"
            )
        };
        var userInfoResponse = await httpClient.SendAsync(userInfoRequest);
        try
        {
            userInfoResponse.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw new ForbiddenException();
        }

        var userInfo =
            JsonConvert.DeserializeObject<GoogleUserDataVM>(await userInfoResponse.Content.ReadAsStringAsync());
        _logger.LogInformation("Calling Google API OK()");
        return userInfo;
    }
}