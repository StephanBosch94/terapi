namespace ULearn.Be.Models.ViewModels.Calendar.Base;

public interface ICalendarBaseTimeslotVM
{
    public string Type { get; }
    public string HostIanaTimezone { get; set; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
}