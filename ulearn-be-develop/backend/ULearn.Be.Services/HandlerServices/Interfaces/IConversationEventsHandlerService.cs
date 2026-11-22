namespace ULearn.Be.Services.HandlerServices.Interfaces;

public interface IConversationEventsHandlerService
{
    Task HandleAdd(int conversationId, int creatorId);
    Task HandleSetRead(int conversationId, int creatorId);
}