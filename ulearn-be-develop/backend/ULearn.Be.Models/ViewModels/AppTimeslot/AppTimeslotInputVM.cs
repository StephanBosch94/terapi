namespace ULearn.Be.Models.ViewModels.AppTimeslot;

public class AppTimeslotInputVM
{
    public int FromLocalYear { get; set; }
    public int FromLocalMonth { get; set; }
    public int FromLocalDay { get; set; }
    public int FromLocalHour { get; set; }
    public int FromLocalMinute { get; set; }
    public int DurationInMinutes { get; set; }
    public string IanaTimezone { get; set; }
}