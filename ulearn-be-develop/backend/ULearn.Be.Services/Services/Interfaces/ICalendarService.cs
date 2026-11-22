using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Calendar;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ICalendarService
{
    Task<PagedVM<CalendarDateEventsVM>> GetTutorPrivateCalendarEvents(
        ClaimsPrincipal principal,
        DateTime from,
        DateTime to,
        bool includeReservations = true,
        bool includeFreeSlots = true
    );

    Task<PagedVM<TutorPrivateCalendarFreeSlotVM>> GetTutorPrivateCalendarFreeSlots(DateTime day,
        string timezone,
        ClaimsPrincipal claimsPrincipal
    );

    Task<PagedVM<TutorPublicCalendarEventVM>> GetTutorPublicCalendarEvents(
        int userId,
        DateTime from,
        DateTime to,
        CClassDurationEnum duration
    );

    Task<CalendarStatisticsVM> GetStatistics(ClaimsPrincipal user);
}