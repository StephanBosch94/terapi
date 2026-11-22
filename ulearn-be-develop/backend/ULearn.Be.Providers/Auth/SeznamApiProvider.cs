using System.Text;
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

public class SeznamApiProvider : ISeznamApiProvider
{
    private readonly ILogger<SeznamApiProvider> _logger;
    private readonly SeznamApiSettings _seznamApiSettings;

    public SeznamApiProvider(
        IEnvConfigurationProvider envConfigurationProvider,
        ILogger<SeznamApiProvider> logger
    )
    {
        _seznamApiSettings =
            envConfigurationProvider.GetSettings<SeznamApiSettings>(EnvSettingsKeyEnum.SeznamApiSettings);
        _logger = logger;
    }

    public async Task<AuthUserFromExternalSourceVM> Authenticate(AuthExternalLoginIM input)
    {
        var tokens = await GetTokens(input);
        var userInfo = await GetUserInfo(tokens.AccessToken);
        return new AuthUserFromExternalSourceVM
        {
            Email = tokens.AccountName,
            Name = userInfo.Username,
            FirstName = userInfo.Firstname,
            LastName = userInfo.Lastname,
            Locale = null,
            PictureUrl = userInfo.AvatarUrl,
            VerifiedEmail = true,
            Phone = userInfo.ContactPhone,
            Tokens = new List<ExternalUserTokenVM>
            {
                new()
                {
                    CTokenProviderEnum = CTokenProviderEnum.SeznamApi,
                    CTokenTypeEnum = CTokenTypeEnum.RefreshToken,
                    TokenValue = tokens.RefreshToken
                },
                new()
                {
                    CTokenProviderEnum = CTokenProviderEnum.SeznamApi,
                    CTokenTypeEnum = CTokenTypeEnum.AccessToken,
                    TokenValue = tokens.AccessToken
                }
            }
        };
    }

    private async Task<SeznamAuthResultVM> GetTokens(AuthExternalLoginIM input)
    {
        _logger.LogInformation("Calling Seznam API getTokens()");
        var data = new Dictionary<string, string>
        {
            { "code", input.AuthenticationCode },
            { "client_id", _seznamApiSettings.ClientId },
            { "client_secret", _seznamApiSettings.ClientSecret },
            { "redirect_uri", input.RedirectUrl },
            { "grant_type", "authorization_code" }
        };
        var httpClient = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://login.szn.cz/api/v1/oauth/token")
        {
            Headers = { { "Accept", "application/json" } },
            Content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            )
        };

        var tokensResponse = await httpClient.SendAsync(request);

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
            JsonConvert.DeserializeObject<SeznamAuthResultVM>(await tokensResponse.Content.ReadAsStringAsync());
        _logger.LogInformation("Calling Seznam API OK()");
        return tokens;
    }

    private async Task<SeznamUserDataVM> GetUserInfo(string accessToken)
    {
        _logger.LogInformation("Calling Seznam API getUserInfo()");
        var httpClient = new HttpClient();
        var userInfoRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://login.szn.cz/api/v1/user"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Authorization", $"Bearer {accessToken}" }
            }
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
            JsonConvert.DeserializeObject<SeznamUserDataVM>(await userInfoResponse.Content.ReadAsStringAsync());
        _logger.LogInformation("Calling Seznam API OK()");
        return userInfo;
    }
}