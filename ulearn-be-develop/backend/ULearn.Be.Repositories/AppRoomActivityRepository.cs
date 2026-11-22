using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppRoomActivityRepository : BaseService, IAppRoomActivityRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppRoomActivityRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task Add(int roomId, int userId, CRoomActivityTypeEnum activityTypeEnum)
    {
        var now = _timeUtility.GetUtcNow();

        var newAppRoomActivity = new AppRoomActivity
        {
            RoomId = roomId,
            UserId = userId,
            RoomActivityTypeId = (int)activityTypeEnum,
            Created = now,
            Updated = now,
        };

        _db.AppRoomActivities.Add(newAppRoomActivity);

        return _db.SaveChangesAsync();
    }

    public Task<bool> Exists(int roomId, int userId, CRoomActivityTypeEnum activityTypeEnum)
    {
        return Query()
            .Where(x => x.RoomId == roomId)
            .Where(x => x.UserId == userId)
            .Where(x => x.RoomActivityTypeId == (int)activityTypeEnum)
            .AnyAsync();
    }

    public Task<AppRoomActivity> GetLatestActivity(int roomId, int userId)
    {
        return Query()
            .Where(x => x.UserId == userId)
            .Where(x => x.RoomId == roomId)
            .OrderByDescending(x => x.RoomActivityId)
            .FirstOrDefaultAsync();
    }

    public Task<CRoomActivityTypeEnum> GetLatestActivityTypeId(int roomId, int userId)
    {
        return Query()
            .Where(x => x.UserId == userId)
            .Where(x => x.RoomId == roomId)
            .OrderByDescending(x => x.RoomActivityId)
            .Select(x => (CRoomActivityTypeEnum)x.RoomActivityTypeId)
            .FirstOrDefaultAsync();
    }

    public async Task Update(int roomActivityId)
    {
        var dbEntity = await Query(false).FirstOrDefaultAsync(x => x.RoomActivityId == roomActivityId);
        ThrowIfNotFound(roomActivityId, dbEntity);

        dbEntity.Updated = _timeUtility.GetUtcNow();
        await _db.SaveChangesAsync();
    }

    private IQueryable<AppRoomActivity> Query(bool asNoTracking = true)
    {
        var query = _db.AppRoomActivities;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}