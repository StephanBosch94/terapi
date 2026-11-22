using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Websocket.Services.Services.Interfaces;

namespace ULearn.Be.Websocket.Services.Services;

public class RoomWebsocketDbService : IRoomWebsocketDbService
{
    private readonly IAppRoomActivityRepository _roomActivityRepository;
    private readonly IAppRoomRepository _roomRepository;

    public RoomWebsocketDbService(IAppRoomActivityRepository roomActivityRepository, IAppRoomRepository roomRepository)
    {
        _roomActivityRepository = roomActivityRepository;
        _roomRepository = roomRepository;
    }

    public async Task HandleUserDisconnected(ClaimsPrincipal contextUser, string roomKey)
    {
        var room = await _roomRepository.GetByKey(roomKey);
        await _roomActivityRepository.Add(room.RoomId, contextUser.GetUserId(), CRoomActivityTypeEnum.Disconnected);
    }
}