using ULearn.Be.Models.ViewModels.Activity;

namespace ULearn.Be.Websocket.Services.HubServices.Interfaces;

public interface IRoomHubService
{
    Task SendMessageToOpponent(int opponentId, OpponentActivityMessageVM message);
}