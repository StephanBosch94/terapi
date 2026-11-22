using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.ViewModels.Calendar.Base;

public enum CalendarEventTypeEnum
{
    [ObjectName("reservation")] Reservation = 1,
    [ObjectName("timeslot")] Timeslot = 2,
    [ObjectName("recurrent-timeslot")] RecurrentTimeslot = 3
}