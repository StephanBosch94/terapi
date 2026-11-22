using Microsoft.AspNetCore.Builder;

namespace ULearn.Be.Websocket.Services.Hubs.Infrastucture;

public static class HubExtension
{
    public static void AddHubs(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ChatHub>("hub/chatHub");
            endpoints.MapHub<RoomHub>("hub/roomHub");
        });
    }
}