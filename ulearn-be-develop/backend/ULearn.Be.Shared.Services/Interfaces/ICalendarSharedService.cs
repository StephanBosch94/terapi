using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Models.ViewModels.Calendar.Base;

namespace ULearn.Be.Shared.Services.Interfaces;

public interface ICalendarSharedService
{
    Task<IReadOnlyCollection<CalendarBaseEventVM>> GetAll(
        int userId,
        DateTime from,
        DateTime to,
        bool includeReservations = true,
        bool includeFreeSlots = true
    );

    Task<IReadOnlyCollection<CalendarDateEventsVM>> GetDateEvents(
        int userId,
        DateTime from,
        DateTime to,
        bool includeReservations,
        bool includeFreeSlots
    );

    Task<IReadOnlyCollection<TutorPublicCalendarEventVM>> GetTutorPublicEvents(
        int userId,
        DateTime from,
        DateTime to,
        CClassDurationEnum durationEnum
    );

    Task<PagedVM<TutorPrivateCalendarFreeSlotVM>> GetTutorPrivateCalendarFreeSlots(
        DateTime day,
        string timezone,
        ClaimsPrincipal claimsPrincipal
    );

    Task<ICollection<CalendarRecurrentTimeslotVM>> GetAllRecurrentTimeslots(DateTime date, string timezone,
        ClaimsPrincipal user);

    Task<CalendarStatisticsVM> GetStatistics(ClaimsPrincipal user);
}