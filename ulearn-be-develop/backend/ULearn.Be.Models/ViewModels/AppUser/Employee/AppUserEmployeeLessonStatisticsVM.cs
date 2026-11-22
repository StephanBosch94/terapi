namespace ULearn.Be.Models.ViewModels.AppUser.Employee;

public class AppUserEmployeeLessonStatisticsVM
{
    public int UserId { get; set; }
    public DateTime? LastLessonFromDate { get; set; }
    public DateTime? LastLessonToDate { get; set; }
}