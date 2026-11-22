using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslot;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Models.ViewModels.Calendar.Base;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppRecurrentTimeslotService : IAppRecurrentTimeslotService
{
    private readonly IAppRecurrentTimeslotBlacklistRepository _appRecurrentTimeslotBlacklistRepository;
    private readonly IAppRecurrentTimeslotRepository _appRecurrentTimeslotRepository;
    private readonly ICalendarSharedService _calendarSharedService;
    private readonly IAppTimeslotRepository _timeslotRepository;
    private readonly ITimeUtility _timeUtility;

    public AppRecurrentTimeslotService(
        IAppRecurrentTimeslotRepository appRecurrentTimeslotRepository,
        IAppRecurrentTimeslotBlacklistRepository appRecurrentTimeslotBlacklistRepository,
        ICalendarSharedService calendarSharedService,
        ITimeUtility timeUtility,
        IAppTimeslotRepository timeslotRepository
    )
    {
        _appRecurrentTimeslotRepository = appRecurrentTimeslotRepository;
        _appRecurrentTimeslotBlacklistRepository = appRecurrentTimeslotBlacklistRepository;
        _calendarSharedService = calendarSharedService;
        _timeUtility = timeUtility;
        _timeslotRepository = timeslotRepository;
    }

    public Task Add(AppRecurrentTimeslotInputVM input, ClaimsPrincipal user)
    {
        return _appRecurrentTimeslotRepository.Add(input, user.GetUserId());
    }

    public async Task Delete(
        int recurrentTimeslotId,
        DateTime eventDate,
        ClaimsPrincipal user
    )
    {
        var now = _timeUtility.GetUtcNow();
        var timeslots =
            await _calendarSharedService.GetAll(user.GetUserId(), now.Date, eventDate.Date, includeReservations: false);
        await _appRecurrentTimeslotBlacklistRepository.DeleteByRecurrentTimeslotId(recurrentTimeslotId);
        await _appRecurrentTimeslotRepository.Delete(recurrentTimeslotId, user.GetUserId());
        await CreateTimeslotsUpToThisDate(
            timeslots
                .OfType<CalendarRecurrentTimeslotVM>()
                .Where(x => x.RecurrentTimeslotId == recurrentTimeslotId)
                .Where(x => x.ToTime.Date < eventDate.Date),
            user.GetUserId()
        );
    }

    public Task<ICollection<CalendarRecurrentTimeslotVM>> Get(DateTime date, string timezone, ClaimsPrincipal user)
    {
        return _calendarSharedService.GetAllRecurrentTimeslots(date, timezone, user);
    }

    private async Task CreateTimeslotsUpToThisDate(IEnumerable<CalendarBaseEventVM> dates, int userId)
    {
        var appTimeslots = dates
            .Select(x => new
            {
                FromLocalTime = _timeUtility.ConvertFromUtcToLocalTime(x.HostIanaTimezone, x.FromTime),
                Event = x
            })
            .Select(x =>
                new AppTimeslot
                {
                    OwnerId = userId,
                    IanaTimezone = x.Event.HostIanaTimezone,
                    DurationInMinutes = (int)(x.Event.ToTime - x.Event.FromTime).TotalMinutes,
                    FromLocalYear = x.FromLocalTime.Year,
                    FromLocalMonth = x.FromLocalTime.Month,
                    FromLocalDay = x.FromLocalTime.Day,
                    FromLocalHour = x.FromLocalTime.Hour,
                    FromLocalMinute = x.FromLocalTime.Minute
                });
        await _timeslotRepository.CreateRange(appTimeslots);
    }
}