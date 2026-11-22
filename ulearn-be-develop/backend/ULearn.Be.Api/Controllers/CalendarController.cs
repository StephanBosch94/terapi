using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/calendar")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    private readonly ILogger<CalendarController> _logger;

    public CalendarController(ICalendarService calendarService, ILogger<CalendarController> logger)
    {
        _calendarService = calendarService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = PolicyConstants.OnlyTutor,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<CalendarDateEventsVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<PagedVM<CalendarDateEventsVM>> GetTutorPrivateCalendarEvents(
        [FromQuery] [Required] DateTime from,
        [FromQuery] [Required] DateTime to,
        [FromQuery] bool includeReservations = true,
        [FromQuery] bool includeFreeSlots = true
    )
    {
        return _calendarService.GetTutorPrivateCalendarEvents(User, from, to, includeReservations, includeFreeSlots);
    }

    /// <summary>
    ///     Returns slots that hasn't been occupied yet based on tutor's configured events
    /// </summary>
    /// <param name="day"></param>
    /// <param name="timezone"></param>
    /// <returns></returns>
    [HttpGet("free-slot")]
    [Authorize(Policy = PolicyConstants.OnlyTutor,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<TutorPrivateCalendarFreeSlotVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<PagedVM<TutorPrivateCalendarFreeSlotVM>> GetTutorPrivateCalendarFreeSlots(
        [FromQuery] [Required] DateTime day,
        [FromQuery] [Required] string timezone
    )
    {
        return _calendarService.GetTutorPrivateCalendarFreeSlots(day, timezone, User);
    }

    /// <summary>
    ///     Returns available time for new tutor's availability slots (the ones that hasn't been created yet)
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    [HttpGet("user/{userId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedVM<TutorPublicCalendarEventVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<PagedVM<TutorPublicCalendarEventVM>> GetTutorPublicCalendarEvents(
        int userId,
        [FromQuery] [Required] DateTime from,
        [FromQuery] [Required] DateTime to,
        [FromQuery] [Required] CClassDurationEnum duration
    )
    {
        return _calendarService.GetTutorPublicCalendarEvents(userId, from, to, duration);
    }

    /// <summary>
    ///     Returns calendar statistics (such as hasConfiguredAvailability) 
    /// </summary>
    /// <returns></returns>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(CalendarStatisticsVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = PolicyConstants.OnlyTutor,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public Task<CalendarStatisticsVM> GetStatistics()
    {
        return _calendarService.GetStatistics(User);
    }
}