namespace ULearn.Be.Models.ViewModels.AppRecurrentTimeslot;

public class AppRecurrentTimeslotInputVM
{
    public int LocalIsoWeekDay { get; set; }
    public int FromLocalHour { get; set; }
    public int FromLocalMinute { get; set; }
    public int DurationInMinutes { get; set; }
    public string IanaTimezone { get; set; }
}