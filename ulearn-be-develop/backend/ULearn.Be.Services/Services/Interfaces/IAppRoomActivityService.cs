using System.Security.Claims;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Activity;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppRoomActivityService
{
    Task Add(string roomKey, ActivityInputVM input, ClaimsPrincipal principal);
}