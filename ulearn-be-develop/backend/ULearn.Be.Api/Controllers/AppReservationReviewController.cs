using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/reservation-review")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppReservationReviewController : ControllerBase
{
    private readonly IAppReservationReviewService _appReservationReviewService;
    private readonly ILogger<AppReservationReviewController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppReservationReviewController(
        IAppReservationReviewService appReservationReviewService,
        ILogger<AppReservationReviewController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appReservationReviewService = appReservationReviewService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(AppReservationReviewIM input)
    {
        await _transactionHandler.WithTransaction(() => _appReservationReviewService.Add(input, User));

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(AppReservationReviewEditIM input)
    {
        await _transactionHandler.WithTransaction(() => _appReservationReviewService.Edit(input, User));

        return NoContent();
    }
}