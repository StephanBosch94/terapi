using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Utils;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/reservation")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppReservationController : CustomControllerBase
{
    private readonly IAppReservationService _appReservationService;
    private readonly IAppUserService _appUserService;
    private readonly ILogger<AppReservationController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppReservationController(
        ILogger<AppReservationController> logger,
        IDbContextTransactionHandler transactionHandler,
        IAppReservationService appReservationService,
        ITokenService tokenService,
        IAppUserService appUserService
    ) : base(tokenService)
    {
        _logger = logger;
        _transactionHandler = transactionHandler;
        _appReservationService = appReservationService;
        _appUserService = appUserService;
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(PagedVM<AppReservationVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<PagedVM<AppReservationVM>> Filter(FilterView<AppReservationFilter> filter)
    {
        return _appReservationService.Filter(filter, User);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(AppReservationCreateVM), StatusCodes.Status200OK)]
    public async Task<AppReservationCreateVM> Create(AppReservationInputVM inputVm)
    {
        var result = await _transactionHandler.WithTransaction(() => _appReservationService.Create(inputVm, User));
        await SignInAsync(AuthUtil.GetClaims(await _appUserService.GetShortInfo(User)));
        return result;
    }

    [HttpPut("{reservationId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Edit(AppReservationEditVM inputVm, int reservationId)
    {
        await _transactionHandler.WithTransaction(() => _appReservationService.Edit(reservationId, inputVm, User));

        return NoContent();
    }
}