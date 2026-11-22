using System.Security.Claims;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Room;
using ULearn.Be.Providers.Video.Interfaces;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppRoomService : IAppRoomService
{
    private readonly IAppRoomActivityRepository _appRoomActivityRepository;
    private readonly IAppRoomRepository _appRoomRepository;
    private readonly IVideoCallProvider _videoCallProvider;

    public AppRoomService(
        IAppRoomRepository appRoomRepository,
        IVideoCallProvider videoCallProvider,
        IAppRoomActivityRepository appRoomActivityRepository
    )
    {
        _appRoomRepository = appRoomRepository;
        _videoCallProvider = videoCallProvider;
        _appRoomActivityRepository = appRoomActivityRepository;
    }

    public async Task<AppRoomVM> GetByKey(string key, ClaimsPrincipal principal)
    {
        var result = await _appRoomRepository.GetByKey(key);
        ThrowIfHasNoAccess(result, principal);

        EnhanceWithAccessToken(result);
        await EnhanceWithIsJoined(result, principal);

        return result;
    }

    private void EnhanceWithAccessToken(AppRoomVM room)
    {
        room.AccessToken = _videoCallProvider.GetToken();
    }

    private async Task EnhanceWithIsJoined(AppRoomVM room, ClaimsPrincipal principal)
    {
        room.IsAlreadyJoined =
            await _appRoomActivityRepository.Exists(room.RoomId, principal.GetUserId(), CRoomActivityTypeEnum.Join);
    }

    private static void ThrowIfHasNoAccess(AppRoomVM result, ClaimsPrincipal principal)
    {
        var userId = principal.GetUserId();
        if (result.Attender.UserId == userId)
        {
            return;
        }

        if (result.Host.UserId == userId)
        {
            return;
        }

        throw new ForbiddenException();
    }
}