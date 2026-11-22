using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Models.ViewModels.Activity;
using ULearn.Be.Websocket.Services.Constants;
using ULearn.Be.Websocket.Services.Hubs;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;

namespace ULearn.Be.Websocket.Services.HubServices;

public class RoomHubService: IRoomHubService
{
    private readonly IHubContext<RoomHub> _hubContext;
    private readonly ILogger<RoomHubService> _logger;

    public RoomHubService(IHubContext<RoomHub> hubContext, ILogger<RoomHubService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendMessageToOpponent(int opponentId, OpponentActivityMessageVM message)
    {
        _logger.LogInformation("Sending new message notification to user {UserId}", opponentId);
        _logger.LogInformation(JsonConvert.SerializeObject(message));
        await _hubContext.Clients.User(opponentId.ToString())
            .SendAsync(HubConstants.RoomOpponentActivity, message);
    }
}