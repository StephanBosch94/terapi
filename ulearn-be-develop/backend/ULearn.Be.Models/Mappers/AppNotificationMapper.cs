using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppNotification;

namespace ULearn.Be.Models.Mappers;

public static class AppNotificationMapper
{
    private static readonly Expression<Func<AppNotification, AppNotificationVM>> AsVM =
        dataItem => new AppNotificationVM
        {
            NotificationId = dataItem.NotificationId,
            RecipientId = dataItem.RecipientId,
            NotificationStateId = dataItem.NotificationStateId,
            Created = dataItem.Created,
            ReadDateTime = dataItem.ReadDateTime,
            JsonData = dataItem.JsonData,
            NotificationState = CNotificationStateMapper.ToVm(dataItem.CNotificationState),
            NotificationType = CNotificationTypeMapper.ToVm(dataItem.CNotificationType),
            Sender = AppUserMapper.ToCommonVm(dataItem.Sender),
        };

    public static IQueryable<AppNotificationVM> ToApiList(this IQueryable<AppNotification> dataCollection)
    {
        return dataCollection
            .Include(x => x.CNotificationState)
            .Include(x => x.CNotificationType)
            .Include(x => x.Sender)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsVM);
    }
}