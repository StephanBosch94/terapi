using System.Security.Claims;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.UserRoomStatistic;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserRoomStatisticRepository
{
    Task Add(UserRoomStatisticInputVM input, ClaimsPrincipal user);
    Task<bool> Contains(CRoomStatisticTypeEnum statisticTypeEnum, int roomId, int userId);
}