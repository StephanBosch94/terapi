using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppNotification;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppNotificationRepository
{
    Task<PagedVM<AppNotificationVM>> Filter(FilterView<AppNotificationFilterVM> filter, int userId);
    Task ReadAll(int userId);
    Task<IReadOnlyCollection<int>> CreateRange(IEnumerable<AppNotificationIM> appNotifications);
    Task Read(int notificationId, int userId);
    Task<IReadOnlyCollection<AppNotificationVM>> GetAll(IEnumerable<int> notificationIds);
}