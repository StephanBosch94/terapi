using Newtonsoft.Json;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppReservationIssue;

public class AppReservationIssueVM: AppReservationIssueFilterVM
{
    public int ReservationIssueId { get; set; }
    public AppReservationIssueReservationVM Reservation { get; set; }
    public new string Description { get; set; }
    public new DateTime Created { get; set; }
    public DateTime? Resolved { get; set; }
    public CReservationIssueStateVM State { get; set; }
    public CReservationIssueTypeVM Type { get; set; }
    
    [JsonIgnore] public new int IssueTypeId { get; set; }
    [JsonIgnore] public new int IssueStateId { get; set; }
}