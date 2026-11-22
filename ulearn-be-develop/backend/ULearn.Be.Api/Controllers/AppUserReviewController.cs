using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-review")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppUserReviewController : ControllerBase
{
    private readonly IAppUserReviewService _appUserReviewService;
    private readonly ILogger<AppUserReviewController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppUserReviewController(IAppUserReviewService appUserReviewService, ILogger<AppUserReviewController> logger,
        IDbContextTransactionHandler transactionHandler)
    {
        _appUserReviewService = appUserReviewService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(AppUserReviewIM input)
    {
        await _transactionHandler.WithTransaction(() => _appUserReviewService.Add(input, User));

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(AppUserReviewUpdateIM input)
    {
        await _transactionHandler.WithTransaction(() => _appUserReviewService.Edit(input, User));

        return NoContent();
    }
}