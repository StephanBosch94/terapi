using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Notification;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserNotificationBlacklistSettingRepository
{
    Task<IReadOnlyCollection<RUserNotificationBlacklistSetting>> GetRawDbEntities(int userId);
    Task EditSettings(IEnumerable<RUserNotificationBlacklistSetting> input, int userId);
    Task<bool> ExistsEmail(int userId, CNotificationTypeEnum notificationTypeEnum);
}