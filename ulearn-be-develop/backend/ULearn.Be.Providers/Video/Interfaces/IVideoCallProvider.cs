using ULearn.Be.Providers.Video.Models;

namespace ULearn.Be.Providers.Video.Interfaces;

public interface IVideoCallProvider
{
    Task<CreateRoomResponseVM> CreateRoom(int reservationDurationInMinutes);
    string GetToken();
}