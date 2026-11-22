using System.Security.Claims;
using ULearn.Be.Models.ViewModels.AppConversation;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppConversationService
{
    Task<IReadOnlyCollection<AppConversationVM>> GetAll(ClaimsPrincipal user);
    Task<AppConversationCreateVM> Create(AppConversationInputVM inputVm, ClaimsPrincipal user);
    Task SetRead(int conversationId, ClaimsPrincipal user);
    Task ValidateExists(int conversationId, int userId);
}