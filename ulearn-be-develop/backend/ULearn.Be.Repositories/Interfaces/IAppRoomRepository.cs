using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Models.ViewModels.Room;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppRoomRepository
{
    Task<AppRoomVM> GetByKey(string key, bool throwIfNotFound = true);
    Task<bool> HasRoomAccess(int roomId, int currentUserId);
    Task<AppRoom> GetRawDbEntity(int roomId, bool throwIfNotFound = true);
    Task Update(int roomId, AppRoomUpdateVM input);
    Task<AppReservationRoomVM> GetById(int activityRoomId, bool throwIfNotFound = true);
    Task<Dictionary<int, AppReservationRoomVM>> GetRoomDictionary(IEnumerable<int> roomIds);
    Task<int> GetRoomId(string roomKey);
    Task<AppRoom> Add(int tutorId, int userId, DateTime from, DateTime to, string roomKey);
}