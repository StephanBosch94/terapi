using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppMessage;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppMessageRepository
{
    Task<int> CreateMessage(int conversationId, int senderId, int receiverId, string textValue);
    Task<PagedVM<AppMessageVM>> Filter(int conversationId, FilterView<MessageFilterVM> filter, int userId);
    Task<AppMessageVM> Get(int messageId, int userId);
    Task SetRead(int conversationId, int receiverId);
    Task<Dictionary<int, int>> GetUnreadMessagesDictionary(IEnumerable<int> conversationIds, int receiverId);
    Task<Dictionary<int, AppMessageVM>> GetLatestMessageDictionary(IEnumerable<int> conversationIds);

    Task<IReadOnlyCollection<AppMessageForNotificationVM>> GetUnreadMessagesWithoutNotification(
        int unreadNewMessageNotificationTimeInMinutes
    );

    Task SetNotified(IEnumerable<int> messageIds);
}