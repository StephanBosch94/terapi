using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserProfileBehaviourRepository: IAppUserProfileBehaviourRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppUserProfileBehaviourRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task<bool> Exists(int userId, int visitedUserId)
    {
        return Query()
            .Where(x => x.UserId == userId)
            .Where(x => x.VisitedUserId == visitedUserId)
            .AnyAsync();
    }

    public Task Create(int userId, int visitedUserId)
    {
        var entity = new AppUserProfileBehaviour
        {
            UserId = userId,
            VisitedUserId = visitedUserId,
            Created = _timeUtility.GetUtcNow()
        };

        _db.AppUserProfileBehaviours.Add(entity);
        return _db.SaveChangesAsync();
    }

    private IQueryable<AppUserProfileBehaviour> Query(bool asNoTracking = true)
    {
        var query = _db.AppUserProfileBehaviours;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}