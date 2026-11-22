using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class AppUserInviteIM
{
    public List<string> Emails { get; set; }
    public CWeeklyGoalEnum? WeeklyGoalId { get; set; } = CWeeklyGoalEnum.OneLessonPerWeek;
}