using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslotBlacklist;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/recurrent-timeslot-blacklist")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppRecurrentTimeslotBlacklistController : ControllerBase
{
    private readonly IAppRecurrentTimeslotBlacklistService _appRecurrentTimeslotBlacklistService;
    private readonly ILogger<AppRecurrentTimeslotBlacklistController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppRecurrentTimeslotBlacklistController(
        IAppRecurrentTimeslotBlacklistService appRecurrentTimeslotBlacklistService,
        ILogger<AppRecurrentTimeslotBlacklistController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appRecurrentTimeslotBlacklistService = appRecurrentTimeslotBlacklistService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }


    /// <summary>
    ///  The endpoint is called when deleting a repeating event with check 'delete only this event'
    /// </summary>
    /// <param name="recurrentTimeslot"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> Add(AppRecurrentTimeslotBlacklistInputVM recurrentTimeslot)
    {
        await _transactionHandler.WithTransaction(() =>
            _appRecurrentTimeslotBlacklistService.Add(recurrentTimeslot, User));

        return NoContent();
    }
}