using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Notification;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRNotificationTypeUserRoleRepository
{
    Task<IReadOnlyCollection<RUserNotificationSettingVM>> GetByRole(CUserRoleEnum userRoleEnum);
}