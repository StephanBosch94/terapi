using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Models.ViewModels.Notification;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppNotificationService
{
    Task<PagedVM<AppNotificationVM>> Filter(FilterView<AppNotificationFilterVM> filter, ClaimsPrincipal user);
    Task ReadAll(ClaimsPrincipal user);
    Task Read(int notificationId, ClaimsPrincipal user);
    Task EditSettings(IReadOnlyCollection<RUserNotificationSettingIM> input, ClaimsPrincipal user);
    Task<IReadOnlyCollection<RUserNotificationSettingVM>> GetSettings(ClaimsPrincipal user);
}