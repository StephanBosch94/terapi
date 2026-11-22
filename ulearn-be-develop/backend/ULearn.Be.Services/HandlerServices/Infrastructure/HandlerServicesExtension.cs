using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Services.HandlerServices.Interfaces;

namespace ULearn.Be.Services.HandlerServices.Infrastructure;

public static class HandlerServicesExtension
{
    public static void AddHandlerServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageEventHandlerService, MessageEventHandlerService>();
        services.AddScoped<IConversationEventsHandlerService, ConversationEventsHandlerService>();
        services.AddScoped<IRoomActivityEventHandlerService, RoomActivityEventHandlerService>();
    }
}