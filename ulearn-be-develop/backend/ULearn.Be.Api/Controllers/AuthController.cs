using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.Auth;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController : CustomControllerBase
{
    private readonly IAuthService _authService;
    private readonly AuthSettings _authSettings;
    private readonly ILogger<AuthController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService,
        IDbContextTransactionHandler transactionHandler,
        ITokenService tokenService,
        IEnvConfigurationProvider envConfigurationProvider
    ) : base(tokenService)
    {
        _logger = logger;
        _authService = authService;
        _transactionHandler = transactionHandler;
        _authSettings = envConfigurationProvider.GetSettings<AuthSettings>(EnvSettingsKeyEnum.AuthSettings);
    }

    [AllowAnonymous]
    [HttpPost("login/google-one-tap")]
    public async Task<IActionResult> LoginGoogleOneTap(GoogleOneTapLoginIM input)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _authService.LoginGoogleOneTap(input)
        );

        await SignInAsync(claims);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login/google")]
    public async Task<IActionResult> LoginGoogle(AuthExternalSignUpVM authExternalSignUpVm)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _authService.LoginGoogle(authExternalSignUpVm)
        );

        await SignInAsync(claims);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login/seznam")]
    public async Task<IActionResult> LoginSeznam(AuthExternalSignUpVM authExternalModel)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _authService.LoginSeznam(authExternalModel)
        );

        await SignInAsync(claims);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login/facebook")]
    public async Task<IActionResult> LoginFacebook(AuthExternalSignUpVM authExternalModel)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _authService.LoginFacebook(authExternalModel)
        );
        await SignInAsync(claims);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginInternal(AuthInternalLoginIM authInternalLoginIm)
    {
        var claims = await _authService.LoginInternal(authInternalLoginIm);
        await SignInAsync(claims);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("bearer-login")]
    public async Task<AuthTokenVM> LoginInternalBearer(AuthInternalLoginIM authInternalLoginIm)
    {
        return await _authService.LoginInternalBearer(authInternalLoginIm);
    }

    [HttpDelete("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Response.Cookies.Delete(CommonConstants.BearerCookieName,
            new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = false,
                Domain = _authSettings.Domain
            });
        return Ok();
    }
}