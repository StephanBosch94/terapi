using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

public abstract class ReservationBaseNotificationVm : NotificationBaseVM
{
    public ReservationBaseNotificationVm()
    {
    }

    public ReservationBaseNotificationVm(AppReservationForNotificationVM reservation)
    {
        ReservationPackageId = reservation.ReservationPackageId;
        ReservationId = reservation.ReservationId;
        StartTime = reservation.StartTime;
        EndTime = reservation.EndTime;
        ExpectedEndTime = reservation.ExpectedEndTime;
        SourceStartTime = reservation.SourceStartTime;
        SourceEndTime = reservation.SourceEndTime;
        SourceExpectedEndTime = reservation.SourceExpectedEndTime;
        RoomKey = reservation.RoomKey;
    }

    public int ReservationPackageId { get; set; }
    public int ReservationId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime ExpectedEndTime { get; set; }
    public DateTime? SourceStartTime { get; set; }
    public DateTime? SourceEndTime { get; set; }
    public DateTime? SourceExpectedEndTime { get; set; }
    public string RoomKey { get; set; }
}