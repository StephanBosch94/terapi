namespace ULearn.Be.Common.Utils;

public static class TimeStaticUtility
{
    public static int ToIsoWeekday(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            DayOfWeek.Sunday => 7,
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
    }
}