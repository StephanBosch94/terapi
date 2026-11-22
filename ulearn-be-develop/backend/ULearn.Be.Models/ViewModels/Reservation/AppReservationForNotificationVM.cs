using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppReservationForNotificationVM
{
    public int ReservationPackageId { get; set; }
    public int ReservationId { get; set; }
    public string RoomKey { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime ExpectedEndTime { get; set; }
    public DateTime? SourceStartTime { get; set; }
    public DateTime? SourceEndTime { get; set; }
    public DateTime? SourceExpectedEndTime { get; set; }
    public int ReservationStateId { get; set; }
    public AppUserCommonVM Host { get; set; }
    public AppUserCommonVM Attender { get; set; }
}