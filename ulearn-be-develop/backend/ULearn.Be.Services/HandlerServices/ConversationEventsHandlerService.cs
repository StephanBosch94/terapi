using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.HandlerServices.Interfaces;
using ULearn.Be.Websocket.Services.Hubs;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;

namespace ULearn.Be.Services.HandlerServices;

public class ConversationEventsHandlerService: IConversationEventsHandlerService
{
    private readonly IAppConversationRepository _appConversationRepository;
    private readonly IAppMessageRepository _appMessageRepository;
    private readonly IChatHubService _chatHubService;

    public ConversationEventsHandlerService(
        IAppConversationRepository appConversationRepository, 
        IAppMessageRepository appMessageRepository, 
        IChatHubService chatHubService
    )
    {
        _appConversationRepository = appConversationRepository;
        _appMessageRepository = appMessageRepository;
        _chatHubService = chatHubService;
    }

    public async Task HandleAdd(int conversationId, int creatorId)
    {
        var result = await _appConversationRepository.Get(conversationId, creatorId);
        await EnhanceWithLatestMessages(result);
        await EnhanceWithCountOfUnreadMessages(result, creatorId);
        await _chatHubService.SendNewConversation(result.Initiator.UserId, result);
        await _chatHubService.SendNewConversation(result.Opponent.UserId, result);
    }

    public async Task HandleSetRead(int conversationId, int creatorId)
    {
        await _chatHubService.SendRefreshCountOfUnreadMessages(creatorId);
    }
    
    private async Task EnhanceWithLatestMessages(AppConversationVM result)
    {
        var latestMessagesDictionary = await _appMessageRepository.GetLatestMessageDictionary(new List<int>{result.ConversationId});
        result.LatestMessage = latestMessagesDictionary.Any()
            ? latestMessagesDictionary[result.ConversationId]
            : null;
    }
    
    private async Task EnhanceWithCountOfUnreadMessages(AppConversationVM result, int receiverId)
    {
        var unreadMessagesDictionary = await _appMessageRepository.GetUnreadMessagesDictionary(new List<int>{result.ConversationId}, receiverId);
        result.CountOfUnreadMessages = unreadMessagesDictionary.Any()
            ? unreadMessagesDictionary[result.ConversationId]
            : 0;
    }
}