using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.Calendar.Base;

namespace ULearn.Be.Models.ViewModels.Calendar;

public class CalendarRecurrentTimeslotVM : CalendarBaseEventVM, ICalendarBaseTimeslotVM
{
    public int RecurrentTimeslotId { get; set; }
    public override string Type => EnumUtils.GetObjectName(CalendarEventTypeEnum.RecurrentTimeslot);
}