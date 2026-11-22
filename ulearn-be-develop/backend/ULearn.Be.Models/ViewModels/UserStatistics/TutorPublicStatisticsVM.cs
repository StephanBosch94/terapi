namespace ULearn.Be.Models.ViewModels.UserStatistics;

public class TutorPublicStatisticsVM
{
    public DateTime? LastVisited { get; set; }
    public bool IsOnline { get; set; }
    public int NumberOfLessons { get; set; }
    public int NumberOfStudents { get; set; }
    public int AttendancePercentage { get; set; }
}