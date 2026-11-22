namespace ULearn.Be.Models.ViewModels.UserStatistics;

public class CompanyStatisticsVM
{
    public int NumberOfEmployees { get; set; }
    public int NumberOfLessonsThisMonth { get; set; }
    public int NumberOfLessonPreviousMonth { get; set; }
    public int NumberOfCompletedLessons { get; set; }
    public int EmployeesWeeklyGoalThisWeek { get; set; }
    public int EmployeesWeeklyGoalPreviousWeek { get; set; }
}