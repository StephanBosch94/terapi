using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/currency")]
public class CCurrencyController : ControllerBase
{
    private readonly ICCurrencySharedService _cCurrencySharedService;
    private readonly ILogger<CCurrencyController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public CCurrencyController(
        ICCurrencySharedService cCurrencySharedService,
        ILogger<CCurrencyController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _cCurrencySharedService = cCurrencySharedService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = PolicyConstants.OnlyAdmin)]
    public async Task<IActionResult> UpdateCurrencies()
    {
        await _transactionHandler.WithTransaction(
            () => _cCurrencySharedService.UpdateCurrencies()
        );

        return NoContent();
    }

    [HttpGet("timezone/{*timezone}")]
    [ProducesResponseType(typeof(CCurrencyVM), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public Task<CCurrencyVM> GetCurrencyByTimezone(string timezone)
    {
        return _cCurrencySharedService.GetCurrencyByTimezone(timezone);
    }

    [HttpGet("ip/{*ip}")]
    [ProducesResponseType(typeof(CCurrencyVM), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public Task<CCurrencyVM> GetCurrencyByIp(string ip)
    {
        return _cCurrencySharedService.GetCurrencyByIp(ip);
    }
}