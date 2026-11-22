using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Notification;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RNotificationTypeUserRoleMapper
{
    private static readonly Expression<Func<RNotificationTypeUserRole, RUserNotificationSettingVM>> AsUserNotificationSettingVM =
        dataItem => new RUserNotificationSettingVM
        {
            NotificationTypeId = dataItem.NotificationTypeId,
            NotificationTypeSettingTitleKey = dataItem.CNotificationType.SettingTitleKey,
            NotificationTypeSettingDescriptionKey = dataItem.CNotificationType.SettingDescriptionKey,
            Excluded = false
        };
    
    public static IQueryable<RUserNotificationSettingVM> ToUserNotificationSettingList(this IQueryable<RNotificationTypeUserRole> dataItems)
    {
        return dataItems.Select(AsUserNotificationSettingVM);
    }
}