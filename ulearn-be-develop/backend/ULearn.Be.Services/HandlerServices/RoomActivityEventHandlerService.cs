using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Utils;
using ULearn.Be.Models.ViewModels.Activity;
using ULearn.Be.Models.ViewModels.Room;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.HandlerServices.Interfaces;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;

namespace ULearn.Be.Services.HandlerServices;

public class RoomActivityEventHandlerService: IRoomActivityEventHandlerService
{
    private readonly IAppRoomRepository _appRoomRepository;
    private readonly IRoomHubService _roomHubService;
    private readonly IAppRoomActivityRepository _appRoomActivityRepository;
    
    public RoomActivityEventHandlerService(
        IAppRoomRepository appRoomRepository, 
        IAppRoomActivityRepository appRoomActivityRepository, 
        IRoomHubService roomHubService
    )
    {
        _appRoomRepository = appRoomRepository;
        _appRoomActivityRepository = appRoomActivityRepository;
        _roomHubService = roomHubService;
    }

    public async Task HandleNewRoomActivity(int roomId, ActivityInputVM input, int userId)
    {
        var room = await _appRoomRepository.GetRawDbEntity(roomId);
        var roomOpponentId = userId == room.HostId ? room.AttenderId : room.HostId;
        var latestHostActivity = await _appRoomActivityRepository.GetLatestActivityTypeId(roomId, room.HostId);
        var latestAttenderActivity = await _appRoomActivityRepository.GetLatestActivityTypeId(roomId, room.AttenderId);
        var newRoomStateId = RoomStateEnumUtil.GetRoomState((CRoomStateEnum) room.RoomStateId, new List<CRoomActivityTypeEnum>{latestHostActivity, latestAttenderActivity}); 
        
        await _appRoomRepository.Update(roomId, new AppRoomUpdateVM { RoomStateId = newRoomStateId, });
        
        await _roomHubService.SendMessageToOpponent(roomOpponentId, 
            new OpponentActivityMessageVM
            {
                ActivityTypeId = input.ActivityTypeId,
                MessagePayload = input.MessagePayload
            }
        );
    }
}