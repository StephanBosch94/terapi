using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.Room;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/room")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppRoomController : ControllerBase
{
    private readonly IAppRoomService _appRoomService;
    private readonly ILogger<AppRoomController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppRoomController(
        ILogger<AppRoomController> logger,
        IDbContextTransactionHandler transactionHandler,
        IAppRoomService appRoomService
    )
    {
        _logger = logger;
        _transactionHandler = transactionHandler;
        _appRoomService = appRoomService;
    }

    [HttpGet("{key}")]
    [ProducesResponseType(typeof(AppRoomVM), StatusCodes.Status200OK)]
    public Task<AppRoomVM> GetByKey(string key)
    {
        return _appRoomService.GetByKey(key, User);
    }
}