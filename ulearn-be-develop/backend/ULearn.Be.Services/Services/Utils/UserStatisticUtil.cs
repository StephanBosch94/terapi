namespace ULearn.Be.Services.Services.Utils;

public static class UserStatisticUtil
{
    public static int GetCurrentWeeklyGoalProgress(int numberOfLessonsThisWeek, int? userGoalId)
    {
        if (userGoalId is null or 0)
        {
            return 0;
        }

        var result = (int)((double)numberOfLessonsThisWeek * 100 / userGoalId)!;
        return result <= 100 ? result : 100;
    }
}