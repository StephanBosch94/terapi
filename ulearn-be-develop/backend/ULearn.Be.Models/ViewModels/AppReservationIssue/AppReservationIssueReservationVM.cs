using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppReservationIssue;

public class AppReservationIssueReservationVM
{
    public AppUserCommonVM Host { get; set; }
    public AppUserCommonVM Attender { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}