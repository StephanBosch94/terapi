namespace ULearn.Be.Models.ViewModels.AppNotification;

public class AppNotificationReservationVM
{
    public int ReservationPackageId { get; set; }
    public int ReservationId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime? SourceStartTime { get; set; }
    public DateTime? SourceEndTime { get; set; }
    public string RoomKey { get; set; }
}