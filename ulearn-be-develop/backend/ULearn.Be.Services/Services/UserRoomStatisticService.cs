using System.Security.Claims;
using ULearn.Be.Models.ViewModels.UserRoomStatistic;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class UserRoomStatisticService : IUserRoomStatisticService
{
    private readonly IRUserRoomStatisticRepository _rUserRoomStatisticRepository;

    public UserRoomStatisticService(IRUserRoomStatisticRepository rUserRoomStatisticRepository)
    {
        _rUserRoomStatisticRepository = rUserRoomStatisticRepository;
    }

    public Task Add(UserRoomStatisticInputVM input, ClaimsPrincipal user)
    {
        return _rUserRoomStatisticRepository.Add(input, user);
    }
}