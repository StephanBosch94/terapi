using System.Security.Claims;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.ViewModels.Activity;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.HandlerServices.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppRoomActivityService: IAppRoomActivityService
{
    private readonly IAppRoomActivityRepository _appRoomActivityRepository;
    private readonly IAppRoomRepository _appRoomRepository;
    private readonly IRoomActivityEventHandlerService _roomActivityEventHandlerService;

    public AppRoomActivityService(
        IAppRoomActivityRepository appRoomActivityRepository, 
        IAppRoomRepository appRoomRepository, 
        IRoomActivityEventHandlerService roomActivityEventHandlerService
    )
    {
        _appRoomActivityRepository = appRoomActivityRepository;
        _appRoomRepository = appRoomRepository;
        _roomActivityEventHandlerService = roomActivityEventHandlerService;
    }

    public async Task Add(string roomKey, ActivityInputVM input, ClaimsPrincipal principal)
    {
        var roomId = await _appRoomRepository.GetRoomId(roomKey);
        var currentUserId = principal.GetUserId();
        await ThrowIfHasNoRoomAccess(roomId, currentUserId);
        
        await _appRoomActivityRepository.Add(roomId, currentUserId, input.ActivityTypeId);
        await _roomActivityEventHandlerService.HandleNewRoomActivity(roomId, input, currentUserId);
    }

    private async Task ThrowIfHasNoRoomAccess(int roomId, int currentUserId)
    {
        if (!await _appRoomRepository.HasRoomAccess(roomId, currentUserId))
        {
            throw new ForbiddenException();
        }
    }
}