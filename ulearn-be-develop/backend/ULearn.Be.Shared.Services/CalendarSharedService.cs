using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.AppTimeslot;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Models.ViewModels.Calendar.Base;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class CalendarSharedService : ICalendarSharedService
{
    private readonly IAppRecurrentTimeslotBlacklistRepository _appRecurrentTimeslotBlacklistRepository;
    private readonly IAppRecurrentTimeslotRepository _appRecurrentTimeslotRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppTimeslotRepository _appTimeslotRepository;
    private readonly ICClassDurationRepository _cClassDurationRepository;
    private readonly ILogger<CalendarSharedService> _logger;
    private readonly ITimeUtility _timeUtility;

    public CalendarSharedService(
        IAppTimeslotRepository appTimeslotRepository,
        IAppRecurrentTimeslotRepository appRecurrentTimeslotRepository,
        IAppRecurrentTimeslotBlacklistRepository appRecurrentTimeslotBlacklistRepository,
        IAppReservationRepository appReservationRepository,
        ITimeUtility timeUtility,
        ILogger<CalendarSharedService> logger,
        ICClassDurationRepository cClassDurationRepository
    )
    {
        _appTimeslotRepository = appTimeslotRepository;
        _appRecurrentTimeslotRepository = appRecurrentTimeslotRepository;
        _appRecurrentTimeslotBlacklistRepository = appRecurrentTimeslotBlacklistRepository;
        _appReservationRepository = appReservationRepository;
        _timeUtility = timeUtility;
        _logger = logger;
        _cClassDurationRepository = cClassDurationRepository;
    }

    public async Task<IReadOnlyCollection<CalendarBaseEventVM>> GetAll(
        int userId,
        DateTime from,
        DateTime to,
        bool includeReservations = true,
        bool includeFreeSlots = true
    )
    {
        var reservations = includeReservations
            ? GetEvents(await _appReservationRepository.GetAllRaw(userId, from.Date.ToUniversalTime(),
                to.Date.ToUniversalTime()))
            : Enumerable.Empty<CalendarBaseEventVM>();

        var freeSlots = includeFreeSlots
            ? GetEvents(
                await _appRecurrentTimeslotRepository.GetAllRaw(userId),
                await _appRecurrentTimeslotBlacklistRepository.GetAllRaw(userId),
                from.Date,
                to.Date
            ).Concat(GetEvents(await _appTimeslotRepository.GetBaseVMs(userId), from.Date.ToUniversalTime(), to))
            : Enumerable.Empty<CalendarBaseEventVM>();

        return freeSlots
            .Concat(reservations)
            .Where(x => x.FromTime >= from)
            .Where(x => x.ToTime <= to)
            .OrderBy(x => x.FromTime)
            .ToList();
    }

    public async Task<IReadOnlyCollection<CalendarDateEventsVM>> GetDateEvents(
        int userId,
        DateTime from,
        DateTime to,
        bool includeReservations,
        bool includeFreeSlots
    )
    {
        var all = await GetAll(userId, from, to, includeReservations, includeFreeSlots);

        return Enumerable.Range(0, (int)(to.Date - from.Date).TotalDays)
            .Select(x => from.Date.AddDays(x))
            .Select(x => new CalendarDateEventsVM
            {
                Date = x,
                Events = all.Where(y => x.Date.Equals(y.FromTime.Date)).ToList()
            })
            .ToList();
    }

    public async Task<IReadOnlyCollection<TutorPublicCalendarEventVM>> GetTutorPublicEvents(
        int userId,
        DateTime from,
        DateTime to,
        CClassDurationEnum durationEnum
    )
    {
        var nextDayDate = _timeUtility.GetUtcNow()
            .AddHours(CommonConstants.MinimumTimeForClassReservationForStudentInHours);
        var alteredFrom = nextDayDate > from ? nextDayDate : from;
        if (alteredFrom > to)
        {
            return new List<TutorPublicCalendarEventVM>();
        }

        var classDuration = await _cClassDurationRepository.Get((int)durationEnum);
        var durationInMinutes = classDuration.DurationWithMargin;
        return (await GetAll(userId, from, to, includeReservations: false))
            .SelectMany(x => Enumerable
                .Range(0, (int)(x.ToTime - x.FromTime).TotalMinutes / durationInMinutes)
                .Select(idx => new EventFromToVM
                {
                    FromLocalTime = x.FromTime.AddMinutes(durationInMinutes * idx),
                    ToLocalTime = x.FromTime.AddMinutes(durationInMinutes * (idx + 1))
                })
            )
            .Where(x => x.FromLocalTime >= alteredFrom)
            .GroupBy(x => x.FromLocalTime.Date)
            .Select(x => new TutorPublicCalendarEventVM
            {
                Date = x.Key,
                Events = x
                    .DistinctBy(y => new { From = y.FromLocalTime, To = y.ToLocalTime })
                    .OrderBy(y => y.FromLocalTime)
                    .ToList()
            })
            .ToList();
    }

    public async Task<PagedVM<TutorPrivateCalendarFreeSlotVM>> GetTutorPrivateCalendarFreeSlots(
        DateTime day,
        string timezone,
        ClaimsPrincipal claimsPrincipal
    )
    {
        var from = _timeUtility.ConvertFromUtcToLocalTime(timezone, day).Date;
        var to = from.AddDays(1);
        var existing = await GetAll(claimsPrincipal.GetUserId(), from.ToUniversalTime(), to.ToUniversalTime());

        var result = Enumerable.Range(0, (int)(to - from).TotalMinutes / CommonConstants.TimeslotDuration)
            .Select(idx => from.AddMinutes(CommonConstants.TimeslotDuration * idx))
            .Select(x => _timeUtility.ConvertFromLocalToUtcTime(timezone, x))
            .Where(x => !existing.Any(y => x >= y.FromTime && x < y.ToTime))
            .Select(x => new TutorPrivateCalendarFreeSlotVM
            {
                FromLocalTime = x,
                ToLocalTime = x.AddMinutes(CommonConstants.TimeslotDuration)
            });
        return new PagedVM<TutorPrivateCalendarFreeSlotVM>(result);
    }

    public async Task<ICollection<CalendarRecurrentTimeslotVM>> GetAllRecurrentTimeslots(
        DateTime date,
        string timezone,
        ClaimsPrincipal user
    )
    {
        var convertedTime = _timeUtility.ConvertFromUtcToLocalTime(timezone, date);
        var timeslots = await _appRecurrentTimeslotRepository.GetAllRaw(user.GetUserId(),
            _timeUtility.ToIsoWeekday(convertedTime.DayOfWeek));
        var startDate = convertedTime.Date;
        return timeslots
            .Select(x => new CalendarRecurrentTimeslotVM
            {
                RecurrentTimeslotId = x.RecurrentTimeslotId,
                FromTime = _timeUtility.ConvertFromLocalToUtcTime(x.IanaTimezone,
                    startDate.AddHours(x.FromLocalHour).AddMinutes(x.FromLocalMinute)),
                ToTime = _timeUtility.ConvertFromLocalToUtcTime(x.IanaTimezone,
                    startDate.AddHours(x.FromLocalHour).AddMinutes(x.FromLocalMinute).AddMinutes(x.DurationInMinutes)),
                HostIanaTimezone = x.IanaTimezone
            }).ToList();
    }

    public async Task<CalendarStatisticsVM> GetStatistics(ClaimsPrincipal user)
    {
        var hasTimeslots = await _appTimeslotRepository.Exists(user.GetUserId());
        var hasRecurrentTimeslots = await _appRecurrentTimeslotRepository.Exists(user.GetUserId());
        return new CalendarStatisticsVM
        {
            HasConfiguredAvailability = hasTimeslots || hasRecurrentTimeslots
        };
    }

    private IEnumerable<CalendarBaseEventVM> GetEvents(
        IEnumerable<AppRecurrentTimeslot> recurrentTimeslots,
        IEnumerable<AppRecurrentTimeslotBlacklist> blacklists,
        DateTime from,
        DateTime to
    )
    {
        var now = _timeUtility.GetUtcNow();
        var repeatingTimeslotsByDayOfWeekId = recurrentTimeslots.ToLookup(x => x.LocalIsoWeekDay);
        var totalDays = (int)(to - from).TotalDays;
        return Enumerable.Range(0, totalDays > 0 ? totalDays : 0)
            .Select(dayIdx => from.Date.AddDays(dayIdx))
            .Where(dateTime => repeatingTimeslotsByDayOfWeekId.Contains(_timeUtility.ToIsoWeekday(dateTime.DayOfWeek)))
            .SelectMany(dateTime => repeatingTimeslotsByDayOfWeekId[_timeUtility.ToIsoWeekday(dateTime.DayOfWeek)]
                .Select(y =>
                    new
                    {
                        DateTime = dateTime,
                        Timeslot = y
                    }))
            .Where(x => blacklists
                .Where(y => y.RecurrentTimeslotId == x.Timeslot.RecurrentTimeslotId)
                .Where(y => x.DateTime.Year == y.LocalYear)
                .Where(y => x.DateTime.Month == y.LocalMonth)
                .All(y => x.DateTime.Day != y.LocalDay)
            )
            .Select(x => new CalendarRecurrentTimeslotVM
            {
                FromTime = _timeUtility.ConvertFromLocalToUtcTime(x.Timeslot.IanaTimezone,
                    x.DateTime.AddHours(x.Timeslot.FromLocalHour).AddMinutes(x.Timeslot.FromLocalMinute)),
                ToTime = _timeUtility.ConvertFromLocalToUtcTime(x.Timeslot.IanaTimezone,
                    x.DateTime.AddHours(x.Timeslot.FromLocalHour)
                        .AddMinutes(x.Timeslot.FromLocalMinute + x.Timeslot.DurationInMinutes)),
                RecurrentTimeslotId = x.Timeslot.RecurrentTimeslotId,
                HostIanaTimezone = x.Timeslot.IanaTimezone
            })
            .Where(x => x.FromTime >= now.Date);
    }

    private IEnumerable<CalendarBaseEventVM> GetEvents(
        IEnumerable<AppTimeslotBaseVM> timeslots,
        DateTime from,
        DateTime to
    )
    {
        return timeslots
            .Select(x => new
            {
                Timeslot = x,
                FromTime = _timeUtility.ConvertFromLocalToUtcTime(x.IanaTimezone,
                    new DateTime(x.FromLocalYear, x.FromLocalMonth, x.FromLocalDay, x.FromLocalHour, x.FromLocalMinute,
                        0, DateTimeKind.Utc))
            })
            .Select(x => new CalendarTimeslotVM
            {
                FromTime = x.FromTime,
                ToTime = x.FromTime.AddMinutes(x.Timeslot.DurationInMinutes),
                TimeslotId = x.Timeslot.TimeslotId,
                HostIanaTimezone = x.Timeslot.IanaTimezone
            })
            .Where(x => x.FromTime >= from)
            .Where(x => x.ToTime <= to);
    }

    private static IEnumerable<CalendarBaseEventVM> GetEvents(IEnumerable<AppReservation> reservations)
    {
        return reservations.Select(x => new CalendarReservationVM
        {
            FromTime = x.StartTime,
            ToTime = x.EndTime,
            ReservationId = x.ReservationId,
            HostIanaTimezone = x.HostEventIanaTimezone
        });
    }
}