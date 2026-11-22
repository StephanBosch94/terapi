using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.Calendar.Base;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.ViewModels.Calendar;

public class CalendarReservationVM : CalendarBaseEventVM
{
    public override string Type => EnumUtils.GetObjectName(CalendarEventTypeEnum.Reservation);
    public int ReservationId { get; set; }
    public AppReservationVM Reservation { get; set; }
}