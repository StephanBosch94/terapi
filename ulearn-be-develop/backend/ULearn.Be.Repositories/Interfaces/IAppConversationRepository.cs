using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppConversation;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppConversationRepository
{
    Task<AppConversation> GetRawDbEntity(int conversationId);
    Task<AppConversation> GetRawDbEntity(IReadOnlyCollection<int> userIds, bool throwIfNotFound = true);
    Task<IReadOnlyCollection<AppConversationVM>> GetAll(int userId);
    Task<AppConversationVM> Get(int conversationId, int userId);
    Task<AppConversation> Create(AppConversationInputVM inputVm, int initiatorId);
    Task<bool> Exists(IReadOnlyCollection<int> userIds);
    Task<bool> Exists(int conversationId, int userId);
    Task<bool> Upsert(int initiatorId, int opponentId);
    Task<IReadOnlyDictionary<int, int>> GetNumberOfUnreadMessagesByConversationId(IEnumerable<int> conversationIds);
}