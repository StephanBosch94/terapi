using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.Mappers.QueryMappers;

public static class ReservationQueryMapper
{
    public static IQueryable<AppReservation> FilterActive(this IQueryable<AppReservation> queryable, DateTime now,
        int timePeriod = 15)
    {
        return queryable
            .Where(x => x.StartTime <= now.AddMinutes(timePeriod) ||
                        x.StartTime <= now) //already running or will be running in 15 minutes
            .Where(x => x.EndTime >= now);
    }

    public static IEnumerable<AppReservationVM> FilterFutureReservations(this IEnumerable<AppReservationVM> queryable,
        DateTime now)
    {
        return queryable
            .Where(x => x.EndTime >= now);
    }

    public static IQueryable<AppReservation> FilterCurrentWeek(this IQueryable<AppReservation> queryable, DateTime now)
    {
        var startWeekTime = now.Date.AddDays(-TimeStaticUtility.ToIsoWeekday(now.DayOfWeek));

        return queryable
            .Where(x => x.StartTime >= startWeekTime)
            .Where(x => x.EndTime <= now);
    }

    public static IQueryable<AppReservation> FilterPreviousWeek(this IQueryable<AppReservation> queryable, DateTime now)
    {
        var endWeekTime = now.Date.AddDays(-TimeStaticUtility.ToIsoWeekday(now.DayOfWeek));
        var startWeekTime = endWeekTime.AddDays(-7);

        return queryable
            .Where(x => x.StartTime >= startWeekTime)
            .Where(x => x.EndTime < endWeekTime);
    }

    public static IQueryable<AppReservation> FilterAttenders(this IQueryable<AppReservation> queryable,
        IEnumerable<int> attenderIds)
    {
        return queryable
            .Where(x => attenderIds.Contains(x.AttenderId));
    }
}