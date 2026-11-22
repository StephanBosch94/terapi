using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppNotificationRepository : BaseService, IAppNotificationRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppNotificationRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<PagedVM<AppNotificationVM>> Filter(FilterView<AppNotificationFilterVM> filter, int userId)
    {
        var results = await Query(userId)
            .ToApiList()
            .ToPagedVMAsync(filter);

        EnhanceWithData(results.Data);

        return results;
    }

    public async Task ReadAll(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var notRead = await Query(userId, false)
            .Where(x => x.NotificationStateId == (int)CNotificationStateEnum.Created)
            .ToListAsync();

        foreach (var appNotification in notRead)
        {
            appNotification.ReadDateTime = now;
            appNotification.NotificationStateId = (int)CNotificationStateEnum.Read;
        }

        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<int>> CreateRange(IEnumerable<AppNotificationIM> appNotifications)
    {
        var now = _timeUtility.GetUtcNow();
        var newNotifications = appNotifications.Select(x =>
            new AppNotification
            {
                RecipientId = x.RecipientId,
                SenderId = x.SenderId,
                NotificationTypeId = (int)x.NotificationTypeEnum,
                NotificationStateId = (int)CNotificationStateEnum.Created,
                Created = now,
                ReadDateTime = null,
                JsonData = JsonConvert.SerializeObject(x.Data,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            }).ToList();

        _db.AddRange(newNotifications);
        await _db.SaveChangesAsync();
        return newNotifications.Select(x => x.NotificationId).ToHashSet();
    }

    public async Task Read(int notificationId, int userId)
    {
        var notification = await Query(userId, false)
            .Where(x => x.NotificationId == notificationId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(notificationId, notification);

        notification.ReadDateTime = _timeUtility.GetUtcNow();
        notification.NotificationStateId = (int)CNotificationStateEnum.Read;

        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<AppNotificationVM>> GetAll(IEnumerable<int> notificationIds)
    {
        var results = await Query()
            .Where(x => notificationIds.Contains(x.NotificationId))
            .ToApiList()
            .ToListAsync();

        EnhanceWithData(results);

        return results;
    }

    private static void EnhanceWithData(IEnumerable<AppNotificationVM> notifications)
    {
        foreach (var appNotificationVm in notifications)
        {
            appNotificationVm.Data =
                JsonConvert.DeserializeObject<NotificationBaseVM>(
                    appNotificationVm.JsonData, new NotificationTemplateConverter()
                );
        }
    }

    private IQueryable<AppNotification> Query(bool asNoTracking = true)
    {
        var query = _db.AppNotifications;

        return asNoTracking ? query.AsNoTracking() : query;
    }

    private IQueryable<AppNotification> Query(int ownerId, bool asNoTracking = true)
    {
        var query = _db.AppNotifications
            .Where(x => x.RecipientId == ownerId);

        return asNoTracking ? query.AsNoTracking() : query;
    }
}