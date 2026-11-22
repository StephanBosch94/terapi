using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.Activity;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/room-activity")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppRoomActivityController : ControllerBase
{
    private readonly IAppRoomActivityService _appRoomActivityService;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppRoomActivityController(
        IAppRoomActivityService appRoomActivityService,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appRoomActivityService = appRoomActivityService;
        _transactionHandler = transactionHandler;
    }

    [HttpPost("room/{roomKey}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(string roomKey, ActivityInputVM input)
    {
        await _transactionHandler.WithTransaction(() => _appRoomActivityService.Add(roomKey, input, User));

        return NoContent();
    }
}