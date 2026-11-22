namespace ULearn.Be.Services.HandlerServices.Interfaces;

public interface IMessageEventHandlerService
{
    Task HandleAdd(int messageId, int creatorId);
}