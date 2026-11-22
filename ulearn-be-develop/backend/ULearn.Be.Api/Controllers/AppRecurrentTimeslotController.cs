using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslot;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/recurrent-timeslot")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppRecurrentTimeslotController : ControllerBase
{
    private readonly IAppRecurrentTimeslotService _appRecurrentTimeslotService;
    private readonly ILogger<AppRecurrentTimeslotController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppRecurrentTimeslotController(
        ILogger<AppRecurrentTimeslotController> logger,
        IAppRecurrentTimeslotService appRecurrentTimeslotService,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _logger = logger;
        _appRecurrentTimeslotService = appRecurrentTimeslotService;
        _transactionHandler = transactionHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Create(AppRecurrentTimeslotInputVM input)
    {
        await _transactionHandler.WithTransaction(() => _appRecurrentTimeslotService.Add(input, User));

        return NoContent();
    }

    [HttpDelete("{recurrentTimeslotId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int recurrentTimeslotId, [Required] [FromQuery] DateTime eventDate)
    {
        await _transactionHandler.WithTransaction(() =>
            _appRecurrentTimeslotService.Delete(recurrentTimeslotId, eventDate, User));

        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<CalendarRecurrentTimeslotVM>), StatusCodes.Status200OK)]
    public Task<ICollection<CalendarRecurrentTimeslotVM>> Get([FromQuery] DateTime date, [FromQuery] string timezone)
    {
        return _appRecurrentTimeslotService.Get(date, timezone, User);
    }
}