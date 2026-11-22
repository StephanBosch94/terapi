namespace ULearn.Be.Models.ViewModels.UserStatistics;

public class StudentStatisticsVM
{
    public int NumberOfCompletedLessonsThisWeek { get; set; }
    public int NumberOfCompletedLessonsPreviousWeek { get; set; }
    public int CurrentWeeklyGoalProgress { get; set; }
    public int TotalWeeklyGoalsCompleted { get; set; }
    public int NumberOfCompletedLessons { get; set; }
    public int NumberOfUpcomingLessons { get; set; }
}