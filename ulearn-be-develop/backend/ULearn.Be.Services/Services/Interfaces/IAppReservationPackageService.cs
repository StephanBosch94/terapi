using System.Security.Claims;
using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppReservationPackageService
{
    Task Delete(int reservationPackageId, ReservationPackageDeleteIM input, ClaimsPrincipal user);
}