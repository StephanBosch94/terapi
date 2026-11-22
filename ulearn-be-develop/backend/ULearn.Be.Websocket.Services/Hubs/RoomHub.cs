using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Activity;
using ULearn.Be.Websocket.Services.Constants;
using ULearn.Be.Websocket.Services.Services.Interfaces;

namespace ULearn.Be.Websocket.Services.Hubs;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class RoomHub : Hub
{
    private const string RoomParameter = "roomKey";
    private readonly ILogger<RoomHub> _logger;
    private readonly IRoomWebsocketDbService _roomWebsocketDbService;

    public RoomHub(ILogger<RoomHub> logger, IRoomWebsocketDbService roomWebsocketDbService)
    {
        _logger = logger;
        _roomWebsocketDbService = roomWebsocketDbService;
    }

    public override async Task OnConnectedAsync()
    {
        string roomKey = Context.GetHttpContext()!.Request.Query[RoomParameter];

        var connectionId = Context.ConnectionId;
        await Groups.AddToGroupAsync(connectionId, roomKey);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string roomKey = Context.GetHttpContext()!.Request.Query[RoomParameter];
        var connectionId = Context.ConnectionId;
        await Groups.RemoveFromGroupAsync(connectionId, roomKey);
        await _roomWebsocketDbService.HandleUserDisconnected(Context.User!, roomKey);
        await Clients.Group(roomKey).SendAsync(HubConstants.RoomOpponentActivity, new OpponentActivityMessageVM
        {
            ActivityTypeId = CRoomActivityTypeEnum.Disconnected,
            MessagePayload = null
        });
        await base.OnDisconnectedAsync(exception);
    }

    // ReSharper disable once UnusedMember.Global
    public Task SentSignalToOpponent(string data)
    {
        string roomKey = Context.GetHttpContext()!.Request.Query[RoomParameter];
        _logger.LogInformation("Sending new message notification in room {RoomKey}", roomKey);
        _logger.LogInformation("{Data}", data);
        return Clients.GroupExcept(roomKey, Context.ConnectionId)
            .SendAsync(HubConstants.RoomOpponentActivity,
                new OpponentActivityMessageVM
                {
                    ActivityTypeId = CRoomActivityTypeEnum.Signal,
                    MessagePayload = data,
                });
    }
}