using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppReservationIssue;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppReservationIssueService
{
    Task Add(ReservationIssueIM input, ClaimsPrincipal user);
    Task ApproveRefund(ReservationIssueApproveIM input, ClaimsPrincipal user);
    Task DeclineRefund(ReservationIssueDecisionIM input, ClaimsPrincipal user);
    Task<PagedVM<AppReservationIssueVM>> Filter(FilterView<AppReservationIssueFilterVM> filter, ClaimsPrincipal user);
}