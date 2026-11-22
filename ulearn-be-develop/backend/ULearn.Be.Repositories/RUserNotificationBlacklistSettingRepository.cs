using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Notification;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserNotificationBlacklistSettingRepository: IRUserNotificationBlacklistSettingRepository
{
    private readonly RepositoryContext _db;

    public RUserNotificationBlacklistSettingRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyCollection<RUserNotificationBlacklistSetting>> GetRawDbEntities(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task EditSettings(IEnumerable<RUserNotificationBlacklistSetting> newEntities, int userId)
    {
        var oldEntities = await Query()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var result = CompareUtil.Compare(
            newEntities.ToList(), 
            oldEntities, 
            x => new { x.NotificationTypeId, x.NotificationChannelId }
        );
        
        _db.RUserNotificationBlacklistSettings.RemoveRange(result.ToRemove);
        _db.RUserNotificationBlacklistSettings.AddRange(result.ToAdd);

        await _db.SaveChangesAsync();
    }

    public Task<bool> ExistsEmail(int userId, CNotificationTypeEnum notificationTypeEnum)
    {
        return Query()
            .Where(x => x.UserId == userId)
            .Where(x => x.NotificationChannelId == (int) CNotificationChannelEnum.Email)
            .Where(x => x.NotificationTypeId == (int)notificationTypeEnum)
            .AnyAsync();
    }

    private IQueryable<RUserNotificationBlacklistSetting> Query(bool asNoTracking = true)
    {
        var query = _db.RUserNotificationBlacklistSettings;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}