using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppReservationIssue;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.Mappers;

public static class AppReservationMapper
{
    private static readonly List<int> ReportIssueStates = new()
    {
        (int)CReservationStateEnum.ScheduledForPayment,
    };

    private static readonly Expression<Func<AppReservation, AppReservationVM>> AsVM =
        dataItem => new AppReservationVM
        {
            ReservationPackageId = dataItem.ReservationPackageId,
            ReservationId = dataItem.ReservationId,
            StartTime = dataItem.StartTime,
            EndTime = dataItem.EndTime,
            ExpectedEndTime = dataItem.ExpectedEndTime,
            Created = dataItem.Created,
            Updated = dataItem.Updated,
            CanReportIssue = ReportIssueStates.Contains(dataItem.ReservationStateId),
            Price = dataItem.Price,
            PriceWithoutSales = dataItem.PriceWithoutSales,
            HostId = dataItem.HostId,
            AttenderId = dataItem.AttenderId,
            ReservationStateId = (CReservationStateEnum)dataItem.ReservationStateId,
            RoomId = dataItem.RoomId,
            Subject = CSubjectMapper.ToVm(dataItem.Subject),
            Duration = CClassDurationMapper.ToVm(dataItem.CClassDuration)
        };

    private static readonly Expression<Func<AppReservation, AppReservationOpponentVM>> AsOpponentVM =
        dataItem => new AppReservationOpponentVM
        {
            ReservationId = dataItem.ReservationId,
            Host = AppUserMapper.ToCommonVm(dataItem.Host),
            Attender = AppUserMapper.ToCommonVm(dataItem.Attender),
        };

    private static readonly Expression<Func<AppReservation, AppReservationForNotificationVM>>
        AsAppReservationForNotificationVM =
            dataItem => new AppReservationForNotificationVM
            {
                ReservationPackageId = dataItem.ReservationPackageId,
                ReservationId = dataItem.ReservationId,
                Host = AppUserMapper.ToCommonVm(dataItem.Host),
                Attender = AppUserMapper.ToCommonVm(dataItem.Attender),
                RoomKey = dataItem.AppRoom.RoomKey,
                StartTime = dataItem.StartTime,
                EndTime = dataItem.EndTime,
                ExpectedEndTime = dataItem.ExpectedEndTime,
                ReservationStateId = dataItem.ReservationStateId,
                SourceStartTime = dataItem.SourceReservation.StartTime,
                SourceEndTime = dataItem.SourceReservation.EndTime,
                SourceExpectedEndTime = dataItem.SourceReservation.ExpectedEndTime
            };

    private static readonly Expression<Func<AppReservation, AppReservationForPaymentVM>>
        AsAppReservationForPaymentVM =
            dataItem => new AppReservationForPaymentVM
            {
                ReservationId = dataItem.ReservationId,
                ReservationPackageId = dataItem.ReservationPackageId,
                SubjectId = dataItem.SubjectId,
                HostId = dataItem.HostId,
                AttenderId = dataItem.AttenderId,
                ReservationStateId = dataItem.ReservationStateId,
                Price = dataItem.Price,
                CurrencyId = dataItem.CurrencyId,
                PriceWithoutSales = dataItem.PriceWithoutSales,
                Created = dataItem.Created,
                Updated = dataItem.Updated,
                RoomId = dataItem.RoomId,
                HostEventIanaTimezone = dataItem.HostEventIanaTimezone,
                SourceReservationId = dataItem.SourceReservationId,
                HasApprovedRefundRequest = dataItem.AppReservationIssues
                    .All(x => x.ReservationIssueStateId != (int)CReservationIssueStateEnum.Approved),
            };

    private static readonly Expression<Func<AppReservation, AppTransactionReservationVM>>
        AsAppTransactionReservationVM =
            dataItem => new AppTransactionReservationVM
            {
                ReservationId = dataItem.ReservationId,
                StartTime = dataItem.StartTime,
                EndTime = dataItem.EndTime
            };

    public static AppReservationForNotificationVM ToAppReservationForNotificationVM(AppReservation dataItem)
    {
        return new AppReservationForNotificationVM
        {
            ReservationPackageId = dataItem.ReservationPackageId,
            ReservationId = dataItem.ReservationId,
            Host = AppUserMapper.ToCommonVm(dataItem.Host),
            Attender = AppUserMapper.ToCommonVm(dataItem.Attender),
            RoomKey = dataItem.AppRoom?.RoomKey,
            StartTime = dataItem.StartTime,
            EndTime = dataItem.EndTime,
            ExpectedEndTime = dataItem.ExpectedEndTime,
            SourceStartTime = dataItem.SourceReservation?.StartTime,
            SourceEndTime = dataItem.SourceReservation?.EndTime,
            SourceExpectedEndTime = dataItem.SourceReservation?.ExpectedEndTime
        };
    }

    public static AppReservationIssueReservationVM ToAppReservationIssueReservationVM(AppReservation dataItem)
    {
        return new AppReservationIssueReservationVM
        {
            Host = AppUserMapper.ToCommonVm(dataItem.Host),
            Attender = AppUserMapper.ToCommonVm(dataItem.Attender),
            StartTime = dataItem.StartTime,
            EndTime = dataItem.EndTime,
        };
    }
    
    public static AppTransactionReservationVM ToAppTransactionReservationVM(AppReservation dataItem)
    {
        return new AppTransactionReservationVM
        {
            ReservationId = dataItem.ReservationId,
            StartTime = dataItem.StartTime,
            EndTime = dataItem.EndTime,
        };
    }
    
    public static IQueryable<AppReservationVM> ToApiList(this IQueryable<AppReservation> dataCollection)
    {
        return dataCollection.Select(AsVM);
    }

    public static IQueryable<AppReservationForNotificationVM> ToAppReservationForNotificationList(
        this IQueryable<AppReservation> dataCollection)
    {
        return dataCollection
            .Include(x => x.Attender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Host)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppReservationForNotificationVM);
    }

    public static IQueryable<AppReservationOpponentVM> ToOpponentList(this IQueryable<AppReservation> dataCollection)
    {
        return dataCollection
            .Include(x => x.Attender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Host)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsOpponentVM);
    }

    public static IQueryable<AppReservationForPaymentVM> ToAppReservationForPaymentList(
        this IQueryable<AppReservation> dataCollection
    )
    {
        return dataCollection
            .Select(AsAppReservationForPaymentVM);
    }

    public static IQueryable<AppTransactionReservationVM> ToAppTransactionReservationList(
        this IQueryable<AppReservation> dataCollection
    )
    {
        return dataCollection
            .Select(AsAppTransactionReservationVM);
    }
}