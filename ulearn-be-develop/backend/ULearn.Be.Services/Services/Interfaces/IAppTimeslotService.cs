using System.Security.Claims;
using ULearn.Be.Models.ViewModels.AppTimeslot;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppTimeslotService
{
    Task Add(AppTimeslotInputVM input, ClaimsPrincipal user);
    Task Delete(int timeslotId, ClaimsPrincipal user);
}