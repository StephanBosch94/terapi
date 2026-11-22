using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Models.ViewModels.AppReservationIssue;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers;

public static class AppReservationIssueMapper
{
    public static readonly Expression<Func<AppReservationIssue, AppReservationIssueForNotificationVM>>
        AsAppReservationIssueForNotificationVM =
            dataItem => new AppReservationIssueForNotificationVM
            {
                ReservationIssueId = dataItem.ReservationIssueId,
                Description = dataItem.Description,
                DecisionText = dataItem.DecisionText,
                Created = dataItem.Created,
                ReservationIssueType = CReservationIssueTypeMapper.ToVm(dataItem.ReservationIssueType),
                ReservationIssueState = CReservationIssueStateMapper.ToVm(dataItem.ReservationIssueState),
                Reservation = AppReservationMapper.ToAppReservationForNotificationVM(dataItem.AppReservation),
                Creator = AppUserMapper.ToCommonVm(dataItem.Creator),
                Resolver = dataItem.Resolver != null
                    ? AppUserMapper.ToCommonVm(dataItem.Resolver)
                    : null
            };

    public static readonly Expression<Func<AppReservationIssue, AppReservationIssueVM>>
        AsAppReservationIssueVM =
            dataItem => new AppReservationIssueVM
            {
                IssueTypeId = dataItem.ReservationIssueTypeId,
                IssueStateId = dataItem.ReservationIssueStateId,
                ReservationIssueId = dataItem.ReservationIssueId,
                Description = dataItem.Description,
                Created = dataItem.Created,
                Reservation = AppReservationMapper.ToAppReservationIssueReservationVM(dataItem.AppReservation),
                Resolved = dataItem.Resolved,
                Type = CReservationIssueTypeMapper.ToVm(dataItem.ReservationIssueType),
                State = CReservationIssueStateMapper.ToVm(dataItem.ReservationIssueState)
            };
    
    public static IQueryable<AppReservationIssueForNotificationVM> ToAppUserReviewForNotificationVmList(
        this IQueryable<AppReservationIssue> dataItems)
    {
        return dataItems
            .Include(x => x.ReservationIssueType)
            .Include(x => x.ReservationIssueState)
            .Include(x => x.Resolver)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Creator)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.AppReservation)
            .ThenInclude(x => x.Attender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.AppReservation)
            .ThenInclude(x => x.Host)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.AppReservation)
            .ThenInclude(x => x.SourceReservation)
            .Include(x => x.AppReservation)
            .ThenInclude(x => x.AppRoom)
            .Select(AsAppReservationIssueForNotificationVM);
    }
    
    public static IQueryable<AppReservationIssueVM> ToApiList(this IQueryable<AppReservationIssue> dataItems)
    {
        return dataItems
            .Include(x => x.ReservationIssueType)
            .Include(x => x.ReservationIssueState)
            .Include(x => x.AppReservation)
            .ThenInclude(x => x.Attender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.AppReservation)
            .ThenInclude(x => x.Host)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppReservationIssueVM);
    }
}