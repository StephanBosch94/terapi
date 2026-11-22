using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Websocket.Services.HubServices;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;
using ULearn.Be.Websocket.Services.Services;
using ULearn.Be.Websocket.Services.Services.Interfaces;

namespace ULearn.Be.Websocket.Services.Hubs.Infrastucture;

public static class HubServicesExtension
{
    public static void AddHubServices(this IServiceCollection services)
    {
        services.AddScoped<IRoomWebsocketDbService, RoomWebsocketDbService>();
        services.AddScoped<IRoomHubService, RoomHubService>();
        services.AddScoped<IChatHubService, ChatHubService>();
    }
}