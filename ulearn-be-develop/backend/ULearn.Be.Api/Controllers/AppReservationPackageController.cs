using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/reservation-package")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppReservationPackageController: ControllerBase
{
    private readonly IAppReservationPackageService _appReservationPackageService;
    private readonly ILogger<AppReservationPackageController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppReservationPackageController(
        IDbContextTransactionHandler transactionHandler, 
        ILogger<AppReservationPackageController> logger, 
        IAppReservationPackageService appReservationPackageService
    )
    {
        _transactionHandler = transactionHandler;
        _logger = logger;
        _appReservationPackageService = appReservationPackageService;
    }
    
    [HttpDelete("{reservationPackageId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int reservationPackageId, ReservationPackageDeleteIM input)
    {
        await _transactionHandler.WithTransaction(() => _appReservationPackageService.Delete(reservationPackageId, input, User));

        return NoContent();
    }
}