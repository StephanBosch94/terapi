using System.Security.Claims;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.HandlerServices.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppConversationService : IAppConversationService
{
    private readonly IAppConversationRepository _appConversationRepository;
    private readonly IAppMessageRepository _appMessageRepository;
    private readonly IConversationEventsHandlerService _conversationEventsHandlerService;

    public AppConversationService(
        IAppConversationRepository appConversationRepository,
        IAppMessageRepository appMessageRepository,
        IConversationEventsHandlerService conversationEventsHandlerService
    )
    {
        _appConversationRepository = appConversationRepository;
        _appMessageRepository = appMessageRepository;
        _conversationEventsHandlerService = conversationEventsHandlerService;
    }

    public async Task<IReadOnlyCollection<AppConversationVM>> GetAll(ClaimsPrincipal user)
    {
        var result = await _appConversationRepository.GetAll(user.GetUserId());

        await EnhanceWithLatestMessages(result);
        await EnhanceWithCountOfUnreadMessages(result, user.GetUserId());

        return OrderConversations(result);
    }

    public async Task<AppConversationCreateVM> Create(AppConversationInputVM inputVm, ClaimsPrincipal user)
    {
        var initiatorId = user.GetUserId();
        var exists = await _appConversationRepository.Exists(new List<int> { initiatorId, inputVm.OpponentId });
        if (exists)
        {
            throw new ConflictException(ErrorCodesEnum.Exists);
        }

        var conversation = await _appConversationRepository.Create(inputVm, initiatorId);
        await _conversationEventsHandlerService.HandleAdd(conversation.ConversationId, initiatorId);
        return new AppConversationCreateVM
        {
            ConversationId = conversation.ConversationId
        };
    }

    public async Task SetRead(int conversationId, ClaimsPrincipal user)
    {
        var userId = user.GetUserId();
        await _appMessageRepository.SetRead(conversationId, userId);
        await _conversationEventsHandlerService.HandleSetRead(conversationId, userId);
    }

    public async Task ValidateExists(int conversationId, int userId)
    {
        var exists = await _appConversationRepository.Exists(conversationId, userId);
        if (!exists)
        {
            throw new ForbiddenException();
        }
    }

    private static IReadOnlyCollection<AppConversationVM> OrderConversations(IEnumerable<AppConversationVM> result)
    {
        return result
            .OrderByDescending(x => x.LatestMessage?.Created)
            .ThenByDescending(x => x.Created)
            .ToList();
    }

    private async Task EnhanceWithLatestMessages(IReadOnlyCollection<AppConversationVM> result)
    {
        var conversationIds = result.Select(x => x.ConversationId);
        var latestMessagesDictionary = await _appMessageRepository.GetLatestMessageDictionary(conversationIds);
        foreach (var appConversationVm in result.Where(x => latestMessagesDictionary.ContainsKey(x.ConversationId)))
        {
            appConversationVm.LatestMessage = latestMessagesDictionary[appConversationVm.ConversationId];
        }
    }

    private async Task EnhanceWithCountOfUnreadMessages(IReadOnlyCollection<AppConversationVM> result, int receiverId)
    {
        var conversationIds = result.Select(x => x.ConversationId);
        var unreadMessagesDictionary =
            await _appMessageRepository.GetUnreadMessagesDictionary(conversationIds, receiverId);
        foreach (var appConversationVm in result.Where(x => unreadMessagesDictionary.ContainsKey(x.ConversationId)))
        {
            appConversationVm.CountOfUnreadMessages = unreadMessagesDictionary[appConversationVm.ConversationId];
        }
    }
}