using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.Calendar.Base;

namespace ULearn.Be.Models.ViewModels.Calendar;

public class CalendarTimeslotVM : CalendarBaseEventVM, ICalendarBaseTimeslotVM
{
    public int TimeslotId { get; set; }
    public override string Type => EnumUtils.GetObjectName(CalendarEventTypeEnum.Timeslot);
}