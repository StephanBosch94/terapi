using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.Wallet;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-wallet")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppUserWalletController : ControllerBase
{
    private readonly IAppUserWalletService _appUserWalletService;
    private readonly ILogger<AppUserWalletController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppUserWalletController(
        IDbContextTransactionHandler transactionHandler,
        ILogger<AppUserWalletController> logger,
        IAppUserWalletService appUserWalletService
    )
    {
        _transactionHandler = transactionHandler;
        _logger = logger;
        _appUserWalletService = appUserWalletService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserWalletVM), StatusCodes.Status200OK)]
    public Task<UserWalletVM> Get()
    {
        return _appUserWalletService.Get(User);
    }

    [HttpPut("deposit")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserWalletDepositVM), StatusCodes.Status200OK)]
    public Task<UserWalletDepositVM> DepositMoney(WalletDepositIM input)
    {
        return _transactionHandler.WithTransaction(() => _appUserWalletService.DepositMoney(input, User));
    }

    [HttpPut("withdrawal")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> WithdrawMoney(WalletWithdrawalIM input)
    {
        await _transactionHandler.WithTransaction(() => _appUserWalletService.WithdrawMoney(input, User));

        return NoContent();
    }

    [HttpPut("top-up")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> TopUpPayment(TopUpPaymentIM input)
    {
        await _transactionHandler.WithTransaction(() => _appUserWalletService.TopUpPayment(input, User));

        return NoContent();
    }

    [HttpPut("user/top-up")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> TopUpUserPayment(TopUpUserPaymentIM input)
    {
        await _transactionHandler.WithTransaction(() => _appUserWalletService.TopUpUserPayment(input, User));

        return NoContent();
    }

    [HttpPut("user/extract")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MoneyExtraction(ExtractPaymentIM input)
    {
        await _transactionHandler.WithTransaction(() => _appUserWalletService.ExtractMoney(input, User));

        return NoContent();
    }
}