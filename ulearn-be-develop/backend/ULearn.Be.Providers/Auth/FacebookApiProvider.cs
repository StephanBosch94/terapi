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

public class FacebookApiProvider : IFacebookProvider
{
    private readonly FacebookApiSettings _facebookApiSettings;
    private readonly ILogger<FacebookApiProvider> _logger;

    public FacebookApiProvider(IEnvConfigurationProvider envConfigurationProvider, ILogger<FacebookApiProvider> logger)
    {
        _logger = logger;
        _facebookApiSettings =
            envConfigurationProvider.GetSettings<FacebookApiSettings>(EnvSettingsKeyEnum.FacebookApiSettings);
    }

    public async Task<AuthUserFromExternalSourceVM> Authenticate(AuthExternalLoginIM input)
    {
        var tokens = await GetTokens(input);
        var userInfo = await GetUserInfo(tokens.AccessToken);
        return new AuthUserFromExternalSourceVM
        {
            Email = userInfo.Email,
            Name = userInfo.Name,
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Locale = null,
            PictureUrl = userInfo.Picture?.Data?.Url,
            VerifiedEmail = true,
            Phone = null,
            Tokens = new List<ExternalUserTokenVM>()
        };
    }

    private async Task<FacebookAuthResultVM> GetTokens(AuthExternalLoginIM input)
    {
        _logger.LogInformation("Calling Facebook API getTokens()");
        var httpClient = new HttpClient();
        var content = new Dictionary<string, string>
        {
            { "code", Uri.EscapeDataString(input.AuthenticationCode) },
            { "client_id", Uri.EscapeDataString(_facebookApiSettings.ClientId) },
            { "client_secret", Uri.EscapeDataString(_facebookApiSettings.ClientSecret) },
            { "redirect_uri", Uri.EscapeDataString(input.RedirectUrl) }
        };
        var parametersString = string.Join("&", content.Select(x => $"{x.Key}={x.Value}"));
        var tokensResponse =
            await httpClient.GetAsync($"https://graph.facebook.com/v14.0/oauth/access_token?{parametersString}");
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
            JsonConvert.DeserializeObject<FacebookAuthResultVM>(await tokensResponse.Content.ReadAsStringAsync());
        _logger.LogInformation("Calling Facebook API OK()");
        return tokens;
    }

    private async Task<FacebookUserDataVM> GetUserInfo(string accessToken)
    {
        _logger.LogInformation("Calling Facebook API getUserInfo()");
        var httpClient = new HttpClient();
        var content = new Dictionary<string, string>
        {
            { "fields", "name,email,birthday,first_name,last_name,picture" },
            { "access_token", Uri.EscapeDataString(accessToken) }
        };
        var parametersString = string.Join("&", content.Select(x => $"{x.Key}={x.Value}"));
        var response = await httpClient.GetAsync($"https://graph.facebook.com/v14.0/me?{parametersString}");
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw new ForbiddenException();
        }

        var userInfo =
            JsonConvert.DeserializeObject<FacebookUserDataVM>(await response.Content.ReadAsStringAsync());
        _logger.LogInformation("Calling Facebook API OK()");
        return userInfo;
    }
}