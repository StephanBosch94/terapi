using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.HandlerServices.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;

namespace ULearn.Be.Services.HandlerServices;

public class MessageEventHandlerService : IMessageEventHandlerService
{
    private readonly IAppMessageRepository _appMessageRepository;
    private readonly IChatHubService _chatHubService;
    private readonly INotificationSharedService _notificationSharedService;

    public MessageEventHandlerService(
        IAppMessageRepository appMessageRepository,
        IChatHubService chatHubService,
        INotificationSharedService notificationSharedService
    )
    {
        _appMessageRepository = appMessageRepository;
        _chatHubService = chatHubService;
        _notificationSharedService = notificationSharedService;
        _chatHubService = chatHubService;
    }

    public async Task HandleAdd(int messageId, int creatorId)
    {
        var appMessageVm = await _appMessageRepository.Get(messageId, creatorId);
        await _chatHubService.SendNewMessage(appMessageVm.SenderId, appMessageVm);
        await _chatHubService.SendNewMessage(appMessageVm.ReceiverId, appMessageVm);
        //await _notificationSharedService.SendNewMessage(appMessageVm.MessageId, appMessageVm.ReceiverId);
    }
}