using System.Security.Claims;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslotBlacklist;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppRecurrentTimeslotBlacklistService
{
    Task Add(AppRecurrentTimeslotBlacklistInputVM input, ClaimsPrincipal user);
}