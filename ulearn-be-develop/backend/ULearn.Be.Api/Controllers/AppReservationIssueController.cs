using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppReservationIssue;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/reservation-issue")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppReservationIssueController : ControllerBase
{
    private readonly IAppReservationIssueService _appReservationIssueService;
    private readonly ILogger<AppReservationIssueController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppReservationIssueController(
        IAppReservationIssueService appReservationIssueService,
        ILogger<AppReservationIssueController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appReservationIssueService = appReservationIssueService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PagedVM<AppReservationIssueVM>), StatusCodes.Status200OK)]
    [HttpPost("filter")]
    public Task<PagedVM<AppReservationIssueVM>> Filter(FilterView<AppReservationIssueFilterVM> filter)
    {
        return _appReservationIssueService.Filter(filter, User);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add(ReservationIssueIM input)
    {
        await _transactionHandler.WithTransaction(() => _appReservationIssueService.Add(input, User));

        return NoContent();
    }

    [HttpPut("approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ApproveRefund(ReservationIssueApproveIM input)
    {
        await _transactionHandler.WithTransaction(() =>
            _appReservationIssueService.ApproveRefund(input, User));

        return Ok();
    }

    [HttpPut("decline")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeclineRefund(ReservationIssueDeclineIM input)
    {
        await _transactionHandler.WithTransaction(() =>
            _appReservationIssueService.DeclineRefund(input, User));

        return Ok();
    }
}