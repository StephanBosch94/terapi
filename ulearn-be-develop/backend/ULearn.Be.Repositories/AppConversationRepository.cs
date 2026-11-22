using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppConversationRepository : BaseService, IAppConversationRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppConversationRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<AppConversation> GetRawDbEntity(int conversationId)
    {
        var result = await Query()
            .Where(x => x.ConversationId == conversationId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(conversationId, result);

        return result;
    }

    public async Task<AppConversation> GetRawDbEntity(IReadOnlyCollection<int> userIds, bool throwIfNotFound = true)
    {
        var result = await Query()
            .Where(x => userIds.Contains(x.InitiatorId))
            .Where(x => userIds.Contains(x.OpponentId))
            .FirstOrDefaultAsync();

        if (throwIfNotFound)
        {
            ThrowIfNotFound("Could not find conversations with specified users", result);
        }

        return result;
    }

    public async Task<IReadOnlyCollection<AppConversationVM>> GetAll(int userId)
    {
        return await Query()
            .Where(x => x.InitiatorId == userId || x.OpponentId == userId)
            .ToApiList()
            .ToListAsync();
    }

    public Task<AppConversationVM> Get(int conversationId, int userId)
    {
        return Query()
            .Where(x => x.ConversationId == conversationId)
            .Where(x => x.InitiatorId == userId || x.OpponentId == userId)
            .ToApiList()
            .FirstOrDefaultAsync();
    }

    public async Task<AppConversation> Create(AppConversationInputVM inputVm, int initiatorId)
    {
        var appConversation = new AppConversation
        {
            InitiatorId = initiatorId,
            OpponentId = inputVm.OpponentId,
            Created = _timeUtility.GetUtcNow(),
            Updated = _timeUtility.GetUtcNow()
        };

        _db.AppConversations.AddRange(appConversation);
        await _db.SaveChangesAsync();
        _db.Entry(appConversation).State = EntityState.Detached;
        return appConversation;
    }

    public Task<bool> Exists(IReadOnlyCollection<int> userIds)
    {
        return Query()
            .Where(x => userIds.Contains(x.InitiatorId))
            .Where(x => userIds.Contains(x.OpponentId))
            .AnyAsync();
    }

    public Task<bool> Exists(int conversationId, int userId)
    {
        return Query()
            .Where(x => x.ConversationId == conversationId)
            .Where(x => x.InitiatorId == userId || x.OpponentId == userId)
            .AnyAsync();
    }

    public async Task<bool> Upsert(int initiatorId, int opponentId)
    {
        var exists = await Exists(new List<int> { initiatorId, opponentId });
        if (exists)
        {
            return false;
        }

        await Create(
            new AppConversationInputVM
            {
                OpponentId = opponentId
            }, initiatorId);

        return true;
    }

    public async Task<IReadOnlyDictionary<int, int>> GetNumberOfUnreadMessagesByConversationId(
        IEnumerable<int> conversationIds)
    {
        var results = await Query()
            .Where(x => conversationIds.Contains(x.ConversationId))
            .Select(x => new
            {
                x.ConversationId,
                CountOfUnreadMessages = x.AppMessages.Count(y => y.MessageStateId == (int)CMessageStateEnum.Created)
            })
            .ToListAsync();

        return results.ToDictionary(x => x.ConversationId, x => x.CountOfUnreadMessages);
    }

    private IQueryable<AppConversation> Query(bool asNoTracking = true)
    {
        var query = _db.AppConversations;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}