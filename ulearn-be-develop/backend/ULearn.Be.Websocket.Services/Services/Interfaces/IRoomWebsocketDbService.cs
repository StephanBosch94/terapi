using System.Security.Claims;

namespace ULearn.Be.Websocket.Services.Services.Interfaces;

public interface IRoomWebsocketDbService
{
    Task HandleUserDisconnected(ClaimsPrincipal contextUser, string roomKey);
}