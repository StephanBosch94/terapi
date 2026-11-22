using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services.Other;

public class TokenService : ITokenService
{
    private readonly AuthSettings _authSettings;

    public TokenService(IEnvConfigurationProvider envConfigurationProvider)
    {
        _authSettings = envConfigurationProvider.GetSettings<AuthSettings>(EnvSettingsKeyEnum.AuthSettings);
    }

    public SecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));

        var token = new JwtSecurityToken(
            issuer: _authSettings.Issuer,
            audience: _authSettings.Issuer,
            expires: DateTime.Now.AddMilliseconds(_authSettings.ExpirationMs),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public CookieOptions GetCookieOptions(DateTimeOffset cookieExpireTime)
    {
        return new CookieOptions
        {
            IsEssential = true,
            Secure = true,
            HttpOnly = false,
            Expires = cookieExpireTime,
            SameSite = SameSiteMode.None,
            Domain = _authSettings.Domain
        };
    }
}