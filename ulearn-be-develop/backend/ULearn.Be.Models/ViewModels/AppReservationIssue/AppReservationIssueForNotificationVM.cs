using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.ViewModels.AppReservationIssue;

public class AppReservationIssueForNotificationVM
{
    public int ReservationIssueId { get; set; }
    public string Description { get; set; }
    public string DecisionText { get; set; }
    public DateTime Created { get; set; }
    public DateTime Resolved { get; set; }
    public AppUserCommonVM Creator { get; set; }
    public AppUserCommonVM Resolver { get; set; }
    public CReservationIssueStateVM ReservationIssueState { get; set; }
    public CReservationIssueTypeVM ReservationIssueType { get; set; }
    public AppReservationForNotificationVM Reservation { get; set; }
}