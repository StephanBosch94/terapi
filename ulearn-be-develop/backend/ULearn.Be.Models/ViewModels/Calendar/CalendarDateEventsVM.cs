using ULearn.Be.Models.ViewModels.Calendar.Base;

namespace ULearn.Be.Models.ViewModels.Calendar;

public class CalendarDateEventsVM
{
    public DateTime Date { get; set; }
    public ICollection<CalendarBaseEventVM> Events { get; set; }
}