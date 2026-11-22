using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class UserBehaviourService : IUserBehaviourService
{
    private readonly IAppConversationRepository _appConversationRepository;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly IAppUserProfileBehaviourRepository _appUserProfileBehaviourRepository;

    public UserBehaviourService(
        INotificationSharedService notificationSharedService,
        IAppConversationRepository appConversationRepository, 
        IAppUserProfileBehaviourRepository appUserProfileBehaviourRepository
    )
    {
        _notificationSharedService = notificationSharedService;
        _appConversationRepository = appConversationRepository;
        _appUserProfileBehaviourRepository = appUserProfileBehaviourRepository;
    }

    public async Task TutorProfileVisited(int userId, ClaimsPrincipal principal)
    {
        if (!principal.IsStudent())
        {
            return;
        }
        var currentUserId = principal.GetUserId();
        var interactionExists = await _appUserProfileBehaviourRepository.Exists(currentUserId, userId);
        var conversationExists = await _appConversationRepository.Exists(new List<int> { currentUserId, userId });
        if (!conversationExists)
        {
            await _appConversationRepository.Create(new AppConversationInputVM { OpponentId = userId }, currentUserId);    
        }

        if (!interactionExists)
        {
            await _notificationSharedService.SendTutorProfileVisited(userId, principal.GetUserId());    
        }
        
        await _appUserProfileBehaviourRepository.Create(currentUserId, userId);
    }
}