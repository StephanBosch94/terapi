using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppReservationPackageService : IAppReservationPackageService
{
    private readonly IAppReservationSharedService _reservationSharedService;

    public AppReservationPackageService(IAppReservationSharedService reservationSharedService)
    {
        _reservationSharedService = reservationSharedService;
    }

    public Task Delete(int reservationPackageId, ReservationPackageDeleteIM input, ClaimsPrincipal user)
    {
        return _reservationSharedService.CancelRemainingReservations(
            reservationPackageId,
            user.GetUserId(),
            input.ReasonText
        );
    }
}