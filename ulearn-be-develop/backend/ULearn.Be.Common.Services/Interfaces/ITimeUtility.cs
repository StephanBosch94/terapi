namespace ULearn.Be.Common.Services.Interfaces;

public interface ITimeUtility
{
    DateTime GetUtcNow();
    DateTime GetNow();
    DateTime GetUtcToday();
    TimeSpan GetUtcOffset(string ianaTimeZone, int year, int month, int day, int hour, int minutes);
    TimeSpan GetOffset(string ianaTimezone, DateTime utcTime);
    DateTime ConvertFromLocalToUtcTime(string ianaTimezone, DateTime localTime);
    DateTime ConvertFromUtcToLocalTime(string ianaTimezone, DateTime utcTime);
    int ToIsoWeekday(DayOfWeek dayOfWeek);
}
