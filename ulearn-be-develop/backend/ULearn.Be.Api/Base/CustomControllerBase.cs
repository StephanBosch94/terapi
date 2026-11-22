using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Base;

public class CustomControllerBase : ControllerBase
{
    private readonly ITokenService _tokenService;

    public CustomControllerBase(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected async Task SignInAsync(ICollection<Claim> claims)
    {
        var cookieExpireTime = DateTimeOffset.UtcNow.AddMonths(1);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = cookieExpireTime,
                RedirectUri = "/api/auth/refresh"
            });

        var token = _tokenService.GetToken(claims);
        HttpContext.Response.Cookies.Append(
            CommonConstants.BearerCookieName,
            new JwtSecurityTokenHandler().WriteToken(token),
            _tokenService.GetCookieOptions(cookieExpireTime)
        );
    }

    protected ClaimsPrincipal GetPrincipalFromCookie()
    {
        var cookie = HttpContext.Request.Cookies[CommonConstants.BearerCookieName];
        if (string.IsNullOrEmpty(cookie))
        {
            return new ClaimsPrincipal();
        }

        var token = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
        return new ClaimsPrincipal(new ClaimsIdentity(token.Claims));
    }

    protected void SetCacheHeaders()
    {
        HttpContext.Response.Headers.Add("Cache-Control", "public, s-maxage=3600, stale-while-revalidate=5400");
    }
}