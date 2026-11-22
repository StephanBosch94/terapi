using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Models.ViewModels.Notification;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppNotificationService : IAppNotificationService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppNotificationRepository _appNotificationRepository;
    private readonly IRUserNotificationBlacklistSettingRepository _rUserNotificationBlacklistSettingRepository;
    private readonly IRNotificationTypeUserRoleRepository _rNotificationTypeUserRoleRepository;
    
    public AppNotificationService(
        IAppNotificationRepository appNotificationRepository, 
        IRUserNotificationBlacklistSettingRepository rUserNotificationBlacklistSettingRepository, 
        IRNotificationTypeUserRoleRepository rNotificationTypeUserRoleRepository, 
        IAppUserRepository appUserRepository
    )
    {
        _appNotificationRepository = appNotificationRepository;
        _rUserNotificationBlacklistSettingRepository = rUserNotificationBlacklistSettingRepository;
        _rNotificationTypeUserRoleRepository = rNotificationTypeUserRoleRepository;
        _appUserRepository = appUserRepository;
    }

    public Task<PagedVM<AppNotificationVM>> Filter(FilterView<AppNotificationFilterVM> filter, ClaimsPrincipal user)
    {
        return _appNotificationRepository.Filter(filter, user.GetUserId());
    }

    public Task ReadAll(ClaimsPrincipal user)
    {
        return _appNotificationRepository.ReadAll(user.GetUserId());
    }

    public Task Read(int notificationId, ClaimsPrincipal user)
    {
        return _appNotificationRepository.Read(notificationId, user.GetUserId());
    }

    public async Task EditSettings(IReadOnlyCollection<RUserNotificationSettingIM> input, ClaimsPrincipal user)
    {
        var userRoleId = await _appUserRepository.GetRoleId(user.GetUserId());
        var allPossible = (await _rNotificationTypeUserRoleRepository.GetByRole(userRoleId))
            .Select(x => x.NotificationTypeId)
            .ToHashSet();
        var includedIds = input.Select(x => x.NotificationTypeId).ToHashSet();
        var excluded = allPossible
            .Except(includedIds)
            .Select(x => new RUserNotificationBlacklistSetting
            {
                UserId = user.GetUserId(),
                NotificationTypeId = x,
                NotificationChannelId = (int)CNotificationChannelEnum.Email
            });
        await _rUserNotificationBlacklistSettingRepository.EditSettings(excluded, user.GetUserId());
    }

    public async Task<IReadOnlyCollection<RUserNotificationSettingVM>> GetSettings(ClaimsPrincipal user)
    {
        var blacklist = await _rUserNotificationBlacklistSettingRepository
            .GetRawDbEntities(user.GetUserId());
        var blacklistedNotificationTypeIds = blacklist.Select(x => x.NotificationTypeId).ToHashSet();
        var userRoleId = await _appUserRepository.GetRoleId(user.GetUserId());
        var results = await _rNotificationTypeUserRoleRepository.GetByRole(userRoleId);
        foreach (var rUserNotificationSettingVm in results.Where(x => blacklistedNotificationTypeIds.Contains(x.NotificationTypeId)))
        {
            rUserNotificationSettingVm.Excluded = true;
        }

        return results;
    }
}