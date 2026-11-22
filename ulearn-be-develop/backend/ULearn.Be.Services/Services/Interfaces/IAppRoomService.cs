using System.Security.Claims;
using ULearn.Be.Models.ViewModels.Room;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppRoomService
{
    Task<AppRoomVM> GetByKey(string key, ClaimsPrincipal principal);
}