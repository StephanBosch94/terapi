namespace ULearn.Be.Models.ViewModels.Calendar;

public class TutorPublicCalendarEventVM
{
    public DateTime Date { get; set; }
    public ICollection<EventFromToVM> Events { get; set; }
}

public class EventFromToVM
{
    public DateTime FromLocalTime { get; set; }
    public DateTime ToLocalTime { get; set; }
}