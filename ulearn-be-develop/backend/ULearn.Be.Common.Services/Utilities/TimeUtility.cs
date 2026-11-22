using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Common.Services.Utilities;

public class TimeUtility : ITimeUtility
{
    private readonly ITimeZoneProvider _timeZoneProvider;

    public TimeUtility(ITimeZoneProvider timeZoneProvider)
    {
        _timeZoneProvider = timeZoneProvider;
    }

    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }

    public DateTime GetNow()
    {
        return DateTime.Now;
    }

    public DateTime GetUtcToday()
    {
        return DateTime.Today.ToUniversalTime();
    }

    public TimeSpan GetUtcOffset(string ianaTimeZone, int year, int month, int day, int hour, int minutes)
    {
        var date = new DateTime(year, month, day, hour, minutes, 0, DateTimeKind.Utc);
        return GetOffset(ianaTimeZone, date);
    }

    public TimeSpan GetOffset(string ianaTimezone, DateTime utcTime)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(ianaTimezone ?? "Europe/Prague");
        return tz.GetUtcOffset(utcTime);
    }

    public DateTime ConvertFromLocalToUtcTime(string ianaTimezone, DateTime localTime)
    {
        var offset = GetOffset(ianaTimezone, localTime);
        return localTime - offset;
    }

    public DateTime ConvertFromUtcToLocalTime(string ianaTimezone, DateTime utcTime)
    {
        var offset = GetOffset(ianaTimezone, utcTime);
        return utcTime + offset;
    }

    public int ToIsoWeekday(DayOfWeek dayOfWeek)
    {
        return TimeStaticUtility.ToIsoWeekday(dayOfWeek);
    }
}