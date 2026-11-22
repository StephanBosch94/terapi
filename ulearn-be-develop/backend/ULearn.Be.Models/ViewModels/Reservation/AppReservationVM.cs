using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppReservationVM : ScheduledEntity
{
    public int ReservationPackageId { get; set; }
    public int ReservationId { get; set; }
    public AppUserCommonVM Opponent { get; set; }
    public bool IsOpponentJoined { get; set; }
    public DateTime ExpectedEndTime { get; set; }
    public CSubjectVM Subject { get; set; }
    public CClassDurationVM Duration { get; set; }
    public CReservationStateEnum ReservationStateId { get; set; }
    public double Price { get; set; }
    public double PriceWithoutSales { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public int OrderNumber { get; set; }
    public int TotalInPackage { get; set; }
    public int RemainingInPackage { get; set; }
    public double RefundPrice { get; set; }
    public bool CanCancel { get; set; }
    public bool CanReschedule { get; set; }
    public bool CanReportIssue { get; set; }
    public AppReservationRoomVM Room { get; set; }
    public AppReservationReviewVM ReservationReview { get; set; }
    public int HostId { get; set; }
    public int AttenderId { get; set; }
    public int RoomId { get; set; }

    public int GetOpponentId(int userId)
    {
        return userId == HostId ? AttenderId : HostId;
    }
}