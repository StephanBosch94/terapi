using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppMessageRepository : BaseService, IAppMessageRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppMessageRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<int> CreateMessage(int conversationId, int senderId, int receiverId, string textValue)
    {
        var appMessage = new AppMessage
        {
            ConversationId = conversationId,
            SenderId = senderId,
            ReceiverId = receiverId,
            TextValue = textValue,
            Created = _timeUtility.GetUtcNow(),
            Updated = _timeUtility.GetUtcNow(),
            MessageStateId = (int)CMessageStateEnum.Created
        };

        _db.AppMessages.Add(appMessage);
        await _db.SaveChangesAsync();

        return appMessage.MessageId;
    }

    public Task<PagedVM<AppMessageVM>> Filter(int conversationId, FilterView<MessageFilterVM> filter, int userId)
    {
        return Query()
            .Where(x => x.ConversationId == conversationId)
            .Where(x => x.SenderId == userId || x.ReceiverId == userId)
            .ToApiList()
            .ToPagedVMAsync(filter);
    }

    public Task<AppMessageVM> Get(int messageId, int userId)
    {
        return Query()
            .Where(x => x.MessageId == messageId)
            .Where(x => x.SenderId == userId || x.ReceiverId == userId)
            .ToApiList()
            .FirstOrDefaultAsync();
    }

    public async Task SetRead(int conversationId, int receiverId)
    {
        var unreadAppMessages = await Query(false)
            .Where(x => x.ConversationId == conversationId)
            .Where(x => x.ReceiverId == receiverId)
            .Where(x => x.MessageStateId == (int)CMessageStateEnum.Created)
            .ToListAsync();

        foreach (var unreadAppMessage in unreadAppMessages)
        {
            unreadAppMessage.MessageStateId = (int)CMessageStateEnum.Read;
        }

        await _db.SaveChangesAsync();
    }

    public Task<Dictionary<int, int>> GetUnreadMessagesDictionary(IEnumerable<int> conversationIds, int receiverId)
    {
        return Query()
            .Where(x => conversationIds.Contains(x.ConversationId))
            .Where(x => x.ReceiverId == receiverId)
            .Where(x => x.MessageStateId == (int)CMessageStateEnum.Created)
            .GroupBy(x => x.ConversationId)
            .Select(x => new
            {
                x.Key,
                Count = x.Count()
            })
            .ToDictionaryAsync(x => x.Key, x => x.Count);
    }

    public Task<Dictionary<int, AppMessageVM>> GetLatestMessageDictionary(IEnumerable<int> conversationIds)
    {
        var latestMessageIdsQuery = Query()
            .Where(x => conversationIds.Contains(x.ConversationId))
            .GroupBy(x => x.ConversationId)
            .Select(x => x.Max(y => y.MessageId));

        return Query()
            .Where(x => latestMessageIdsQuery.Contains(x.MessageId))
            .ToApiList()
            .ToDictionaryAsync(x => x.ConversationId, x => x);
    }

    public async Task<IReadOnlyCollection<AppMessageForNotificationVM>> GetUnreadMessagesWithoutNotification(
        int unreadNewMessageNotificationTimeInMinutes
    )
    {
        var time = _timeUtility.GetUtcNow().AddMinutes(-unreadNewMessageNotificationTimeInMinutes);

        return await Query()
            .Where(x => x.Created < time)
            .Where(x => x.MessageStateId == (int)CMessageStateEnum.Created)
            .Where(x => x.Notified == null)
            .ToAppMessageForNotificationList()
            .ToListAsync();
    }

    public async Task SetNotified(IEnumerable<int> messageIds)
    {
        var now = _timeUtility.GetUtcNow();

        var entities = await Query(false)
            .Where(x => messageIds.Contains(x.MessageId))
            .ToListAsync();

        foreach (var appMessage in entities)
        {
            appMessage.Notified = now;
        }

        await _db.SaveChangesAsync();
    }

    private IQueryable<AppMessage> Query(bool asNoTracking = true)
    {
        var query = _db.AppMessages;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}