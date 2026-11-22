using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Models.ViewModels.AppNotification;

namespace ULearn.Be.Websocket.Services.HubServices.Interfaces;

public interface IChatHubService
{
    Task SendNewMessage(int userId, AppMessageVM message);
    Task SendNewConversation(int userId, AppConversationVM conversation);
    Task SendRefreshCountOfUnreadMessages(int userId);
    Task SendNewNotification(AppNotificationVM notification);
    Task SendNewNotification(IEnumerable<AppNotificationVM> notifications);
}