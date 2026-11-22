using ULearn.Be.Data;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppRoomActivityRepository
{
    Task Add(int roomId, int userId, CRoomActivityTypeEnum activityTypeEnum);
    Task<bool> Exists(int roomId, int userId, CRoomActivityTypeEnum activityTypeEnum);
    Task<CRoomActivityTypeEnum> GetLatestActivityTypeId(int roomId, int userId);
    Task<AppRoomActivity> GetLatestActivity(int roomId, int userId);
    Task Update(int roomActivityId);
}