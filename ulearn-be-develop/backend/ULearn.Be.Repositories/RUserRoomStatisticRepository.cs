using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.UserRoomStatistic;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserRoomStatisticRepository : IRUserRoomStatisticRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public RUserRoomStatisticRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task Add(UserRoomStatisticInputVM input, ClaimsPrincipal user)
    {
        var newEntity = new RUserRoomStatistic
        {
            RoomId = input.RoomId,
            UserId = user.GetUserId(),
            RoomStatisticsTypeId = (int)input.RoomStatisticTypeId,
            Created = _timeUtility.GetUtcNow(),
            TextValue = input.TextValue,
            Value = input.Value,
        };

        _db.RUserRoomStatistics.Add(newEntity);

        await _db.SaveChangesAsync();
    }

    public Task<bool> Contains(CRoomStatisticTypeEnum statisticTypeEnum, int roomId, int userId)
    {
        return Query()
            .Where(x => x.UserRoomStatisticId == (int)statisticTypeEnum)
            .Where(x => x.RoomId == roomId)
            .Where(x => x.UserId == userId)
            .AnyAsync();
    }

    private IQueryable<RUserRoomStatistic> Query(bool asNoTracking = true)
    {
        var query = _db.RUserRoomStatistics;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}