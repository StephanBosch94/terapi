using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUserRegistration;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/registration")]
public class RegistrationController : CustomControllerBase
{
    private readonly ILogger<RegistrationController> _logger;
    private readonly IRegistrationService _registrationService;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public RegistrationController(
        ILogger<RegistrationController> logger,
        IRegistrationService registrationService,
        IDbContextTransactionHandler transactionHandler,
        ITokenService tokenService
    ) : base(tokenService)
    {
        _logger = logger;
        _registrationService = registrationService;
        _transactionHandler = transactionHandler;
    }

    [HttpPost("invite")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> InviteEmployees(AppUserInviteIM input)
    {
        await _transactionHandler.WithTransaction(
            () => _registrationService.InviteEmployees(input, User)
        );

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegistrationIM registrationIm)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _registrationService.Register(registrationIm)
        );

        await SignInAsync(claims);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("forgotten-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgottenPassword(ForgottenPasswordInputVM input)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _registrationService.ForgottenPassword(input)
        );

        return Ok();
    }

    [HttpPost("resend-code")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(UserRegistrationCodeInfoVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<UserRegistrationCodeInfoVM> ResendCode()
    {
        return _transactionHandler.WithTransaction(
            () => _registrationService.ResendCode(User)
        );
    }

    [HttpPut("link/activate")]
    [Authorize(Policy = PolicyConstants.RegistrationCodeConfirmation)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LinkActivate(RegistrationActivationIM registrationActivationIm)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _registrationService.Activate(registrationActivationIm, User)
        );

        await SignInAsync(claims);

        return NoContent();
    }

    [HttpPut("activate")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Activate(RegistrationActivationIM registrationActivationIm)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _registrationService.Activate(registrationActivationIm, User)
        );

        await SignInAsync(claims);

        return NoContent();
    }

    [HttpGet("code-info")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public Task<UserRegistrationCodeInfoVM> GetLatestInfo()
    {
        return _registrationService.GetLatestCodeInfo(User);
    }
}