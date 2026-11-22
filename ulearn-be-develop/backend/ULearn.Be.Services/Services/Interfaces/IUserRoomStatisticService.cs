using System.Security.Claims;
using ULearn.Be.Models.ViewModels.UserRoomStatistic;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IUserRoomStatisticService
{
    Task Add(UserRoomStatisticInputVM input, ClaimsPrincipal user);
}