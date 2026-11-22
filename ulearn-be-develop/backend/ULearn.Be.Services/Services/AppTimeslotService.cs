using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.ViewModels.AppTimeslot;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppTimeslotService : IAppTimeslotService
{
    private readonly IAppTimeslotRepository _appTimeslotRepository;

    public AppTimeslotService(IAppTimeslotRepository appTimeslotRepository)
    {
        _appTimeslotRepository = appTimeslotRepository;
    }

    public Task Add(AppTimeslotInputVM input, ClaimsPrincipal user)
    {
        return _appTimeslotRepository.Add(input, user.GetUserId());
    }

    public Task Delete(int timeslotId, ClaimsPrincipal user)
    {
        return _appTimeslotRepository.Delete(timeslotId, user.GetUserId());
    }
}