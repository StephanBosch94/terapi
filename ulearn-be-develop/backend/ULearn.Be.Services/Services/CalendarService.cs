using System.Security.Claims;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class CalendarService : ICalendarService
{
    private readonly IAppReservationSharedService _appReservationSharedService;
    private readonly IAppUserRepository _appUserRepository;
    private readonly ICalendarSharedService _calendarSharedService;

    public CalendarService(
        ICalendarSharedService calendarSharedService,
        IAppUserRepository appUserRepository,
        IAppReservationSharedService appReservationSharedService
    )
    {
        _calendarSharedService = calendarSharedService;
        _appUserRepository = appUserRepository;
        _appReservationSharedService = appReservationSharedService;
    }

    public async Task<PagedVM<CalendarDateEventsVM>> GetTutorPrivateCalendarEvents(
        ClaimsPrincipal principal,
        DateTime from,
        DateTime to,
        bool includeReservations = true,
        bool includeFreeSlots = true
    )
    {
        var results = await _calendarSharedService.GetDateEvents(
            principal.GetUserId(),
            from,
            to,
            includeReservations,
            includeFreeSlots
        );

        await EnhanceWithReservationInfo(
            results
                .Where(x => x.Events?.Any() is true)
                .SelectMany(x => x.Events)
                .OfType<CalendarReservationVM>()
                .ToList(),
            principal
        );

        return new PagedVM<CalendarDateEventsVM>(results);
    }

    public Task<PagedVM<TutorPrivateCalendarFreeSlotVM>> GetTutorPrivateCalendarFreeSlots(
        DateTime day,
        string timezone,
        ClaimsPrincipal claimsPrincipal
    )
    {
        return _calendarSharedService.GetTutorPrivateCalendarFreeSlots(day, timezone, claimsPrincipal);
    }

    public async Task<PagedVM<TutorPublicCalendarEventVM>> GetTutorPublicCalendarEvents(
        int userId,
        DateTime from,
        DateTime to,
        CClassDurationEnum duration
    )
    {
        await ThrowIfNotTutor(userId);
        var result = await _calendarSharedService.GetTutorPublicEvents(userId, from, to, duration);
        return new PagedVM<TutorPublicCalendarEventVM>(result);
    }

    public Task<CalendarStatisticsVM> GetStatistics(ClaimsPrincipal user)
    {
        return _calendarSharedService.GetStatistics(user);
    }

    private async Task ThrowIfNotTutor(int userId)
    {
        var userRoleId = await _appUserRepository.GetRoleId(userId);
        if (userRoleId != CUserRoleEnum.Tutor)
        {
            throw new ForbiddenException(ErrorCodesEnum.UserIsNotATutor);
        }
    }

    private async Task EnhanceWithReservationInfo(
        IReadOnlyCollection<CalendarReservationVM> resultsData,
        ClaimsPrincipal user
    )
    {
        var reservationIds = resultsData
            .Select(x => x.ReservationId)
            .ToHashSet();
        var reservationDictionary = await _appReservationSharedService.GetAll(reservationIds, user);

        foreach (var userEventVm in resultsData)
        {
            userEventVm.Reservation = reservationDictionary[userEventVm.ReservationId];
        }
    }
}