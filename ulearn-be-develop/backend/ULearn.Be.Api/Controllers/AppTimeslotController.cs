using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.AppTimeslot;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/timeslot")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppTimeslotController : ControllerBase
{
    private readonly IAppTimeslotService _appTimeslotService;
    private readonly ILogger<AppTimeslotController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppTimeslotController(
        IAppTimeslotService appTimeslotService,
        ILogger<AppTimeslotController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appTimeslotService = appTimeslotService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Create(AppTimeslotInputVM input)
    {
        await _transactionHandler.WithTransaction(() => _appTimeslotService.Add(input, User));

        return NoContent();
    }

    [HttpDelete("{timeslotId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int timeslotId)
    {
        await _transactionHandler.WithTransaction(() => _appTimeslotService.Delete(timeslotId, User));

        return NoContent();
    }
}