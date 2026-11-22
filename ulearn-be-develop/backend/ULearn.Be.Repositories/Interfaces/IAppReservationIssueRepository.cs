using ULearn.Be.Common.Models;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppReservationIssue;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppReservationIssueRepository
{
    Task<int> Add(ReservationIssueIM input, int userId);
    Task<AppReservationIssue> GetRawDbEntity(int reservationIssueId);
    Task<AppReservationIssueForNotificationVM> GetNotificationVmById(int reservationIssueId);
    Task Resolve(int reservationIssueId, CReservationIssueStateEnum stateEnum, string decisionText, int userId);
    Task<PagedVM<AppReservationIssueVM>> Filter(FilterView<AppReservationIssueFilterVM> filter);
}