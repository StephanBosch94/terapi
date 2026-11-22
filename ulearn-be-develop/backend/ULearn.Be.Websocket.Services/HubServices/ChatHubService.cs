using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Websocket.Services.Constants;
using ULearn.Be.Websocket.Services.Hubs;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;

namespace ULearn.Be.Websocket.Services.HubServices;

public class ChatHubService : IChatHubService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<ChatHubService> _logger;

    public ChatHubService(IHubContext<ChatHub> hubContext, ILogger<ChatHubService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNewMessage(int userId, AppMessageVM message)
    {
        _logger.LogDebug("Sending new message notification to user {UserId}", userId);
        await _hubContext.Clients.User(userId.ToString())
            .SendAsync(HubConstants.NewMessage, message);
    }

    public async Task SendNewConversation(int userId, AppConversationVM conversation)
    {
        _logger.LogDebug("Sending new conversation notification to user {UserId}", userId);
        await _hubContext.Clients.User(userId.ToString())
            .SendAsync(HubConstants.NewConversation, conversation);
    }

    public async Task SendRefreshCountOfUnreadMessages(int userId)
    {
        _logger.LogDebug("Sending refresh count of unread messages to user {UserId}", userId);
        await _hubContext.Clients.User(userId.ToString())
            .SendAsync(HubConstants.RefreshCountOfUnreadMessages);
    }

    public async Task SendNewNotification(AppNotificationVM notification)
    {
        _logger.LogDebug("Sending new notification to user {UserId}", notification.RecipientId);
        await _hubContext.Clients.User(notification.RecipientId.ToString())
            .SendAsync(HubConstants.NewNotification, notification);
    }

    public async Task SendNewNotification(IEnumerable<AppNotificationVM> notifications)
    {
        foreach (var appNotificationVm in notifications)
        {
            await SendNewNotification(appNotificationVm);
        }
    }
}