using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.UserRoomStatistic;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-room-statistic")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class UserRoomStatisticController : ControllerBase
{
    private readonly ILogger<UserRoomStatisticController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;
    private readonly IUserRoomStatisticService _userRoomStatisticService;

    public UserRoomStatisticController(
        ILogger<UserRoomStatisticController> logger,
        IDbContextTransactionHandler transactionHandler,
        IUserRoomStatisticService userRoomStatisticService
    )
    {
        _logger = logger;
        _transactionHandler = transactionHandler;
        _userRoomStatisticService = userRoomStatisticService;
    }

    /// <summary>
    ///     Stores statistics such as video call review
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Add(UserRoomStatisticInputVM input)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _userRoomStatisticService.Add(input, User)
        );
        return Ok();
    }
}