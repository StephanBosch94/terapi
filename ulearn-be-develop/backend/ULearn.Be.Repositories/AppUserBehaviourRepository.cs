using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserBehaviourRepository : IAppUserBehaviourRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppUserBehaviourRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task AddConnected(string uuid, string url, int userId)
    {
        var newEntity = new AppUserBehaviour
        {
            ConnectionUuid = uuid,
            UserId = userId,
            Url = url,
            Connected = _timeUtility.GetUtcNow(),
            Disconnected = null,
        };

        _db.AppUserBehaviourTransactions.Add(newEntity);
        return _db.SaveChangesAsync();
    }

    public async Task UpdateSetDisconnected(string uuid)
    {
        var entity = await Query(false)
            .Where(x => x.ConnectionUuid == uuid)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return;
        }

        entity.Disconnected = _timeUtility.GetUtcNow();
        await _db.SaveChangesAsync();
    }

    public Task<bool> IsOnline(int userId)
    {
        var nowDate = _timeUtility.GetUtcNow().Date;
        return Query()
            .Where(x => x.UserId == userId)
            .Where(x => x.Connected >= nowDate)
            .Where(x => x.Disconnected == null)
            .AnyAsync();
    }

    public async Task<DateTime> GetLastVisited(int userId)
    {
        var result = await Query()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Connected)
            .Select(x => new { x.Connected, x.Disconnected })
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return DateTime.MinValue;
        }

        return result.Disconnected ?? result.Connected;
    }

    public async Task<IReadOnlyCollection<int>> GetOnlineUserIds(IReadOnlyCollection<int> userIds)
    {
        var nowDate = _timeUtility.GetUtcNow().Date;
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .Where(x => x.Connected >= nowDate)
            .Where(x => x.Disconnected == null)
            .Select(x => x.UserId)
            .ToListAsync();

        return results.ToHashSet();
    }

    public async Task<IReadOnlyDictionary<int, DateTime>> GetLastVisitedByUserId(IReadOnlyCollection<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .GroupBy(x => x.UserId)
            .Select(x => new
            {
                UserId = x.Key,
                LastConnected = x.Max(y => y.Connected),
                LastDisconnected = x.Max(y => y.Disconnected)
            })
            .ToListAsync();

        return results.ToDictionary(
            x => x.UserId,
            x => x.LastDisconnected.HasValue && x.LastDisconnected.Value > x.LastConnected
                ? x.LastDisconnected.Value
                : x.LastConnected
        );
    }
    
    private IQueryable<AppUserBehaviour> Query(bool asNoTracking = true)
    {
        var query = _db.AppUserBehaviourTransactions;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}