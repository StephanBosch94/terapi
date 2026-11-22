using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslotBlacklist;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppRecurrentTimeslotBlacklistService : IAppRecurrentTimeslotBlacklistService
{
    private readonly IAppRecurrentTimeslotBlacklistRepository _appRecurrentTimeslotBlacklistRepository;
    private readonly IAppRecurrentTimeslotRepository _appRecurrentTimeslotService;

    public AppRecurrentTimeslotBlacklistService(
        IAppRecurrentTimeslotBlacklistRepository appRecurrentTimeslotBlacklistRepository,
        IAppRecurrentTimeslotRepository appRecurrentTimeslotService
    )
    {
        _appRecurrentTimeslotBlacklistRepository = appRecurrentTimeslotBlacklistRepository;
        _appRecurrentTimeslotService = appRecurrentTimeslotService;
    }

    public async Task Add(AppRecurrentTimeslotBlacklistInputVM input, ClaimsPrincipal user)
    {
        var repeatingTimeslot = await _appRecurrentTimeslotService.Get(input.RecurrentTimeslotId, user.GetUserId());
        await _appRecurrentTimeslotBlacklistRepository.Add(repeatingTimeslot, input.LocalYear, input.LocalMonth,
            input.LocalDay);
    }
}