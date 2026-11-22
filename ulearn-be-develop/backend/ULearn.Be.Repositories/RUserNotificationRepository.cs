using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserNotificationRepository : IRUserNotificationRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public RUserNotificationRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task Create(int recipientId, CNotificationChannelEnum channelEnum,
        CNotificationTypeEnum notificationTypeEnum, CNotificationStateEnum stateEnum)
    {
        var entity = new RUserNotification
        {
            UserId = recipientId,
            NotificationChannelId = (int)channelEnum,
            NotificationTypeId = (int)notificationTypeEnum,
            NotificationStateId = (int)stateEnum,
            Created = _timeUtility.GetUtcNow()
        };

        _db.RUserNotifications.Add(entity);
        return _db.SaveChangesAsync();
    }

    private IQueryable<RUserNotification> Query(bool asNoTracking = true)
    {
        var query = _db.RUserNotifications;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}