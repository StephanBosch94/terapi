using ULearn.Be.Models.ViewModels.Activity;

namespace ULearn.Be.Services.HandlerServices.Interfaces;

public interface IRoomActivityEventHandlerService
{
    Task HandleNewRoomActivity(int roomId, ActivityInputVM input, int userId);
}