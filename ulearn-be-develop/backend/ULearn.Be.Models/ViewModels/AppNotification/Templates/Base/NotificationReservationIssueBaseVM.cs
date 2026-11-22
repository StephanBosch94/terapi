using ULearn.Be.Models.ViewModels.AppReservationIssue;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

public abstract class NotificationReservationIssueBaseVM : NotificationBaseVM
{
    public NotificationReservationIssueBaseVM(AppReservationIssueForNotificationVM entity)
    {
        ReservationId = entity.Reservation.ReservationId;
        ReservationStartTime = entity.Reservation.StartTime;
        ReservationEndTime = entity.Reservation.EndTime;
        ReservationExpectedEndTime = entity.Reservation.ExpectedEndTime;
        ReservationIssueId = entity.ReservationIssueId;
        Created = entity.Created;
        Resolved = entity.Resolved;
        DecisionText = entity.DecisionText;
        IssueTypeNameKey = entity.ReservationIssueType.NameKey;
        IssueDescription = entity.Description;
    }

    public NotificationReservationIssueBaseVM()
    {
    }

    public int ReservationId { get; set; }
    public DateTime ReservationStartTime { get; set; }
    public DateTime ReservationEndTime { get; set; }
    public DateTime ReservationExpectedEndTime { get; set; }
    public int ReservationIssueId { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Resolved { get; set; }
    public string DecisionText { get; set; }
    public string IssueTypeNameKey { get; set; }
    public string IssueDescription { get; set; }
    public double RefundAmount { get; set; }
    public string RefundAmountCurrencyEnumKey { get; set; }
}