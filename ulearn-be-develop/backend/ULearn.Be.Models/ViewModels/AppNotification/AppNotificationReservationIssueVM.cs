using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppNotification;

public class AppNotificationReservationIssueVM
{
    public int ReservationIssueId { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Resolved { get; set; }
    public string DecisionText { get; set; }
    public CReservationIssueStateVM State { get; set; }
}