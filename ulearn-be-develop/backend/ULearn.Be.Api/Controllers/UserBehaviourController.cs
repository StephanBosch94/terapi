using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-behaviour")]
public class UserBehaviourController : ControllerBase
{
    private readonly ILogger<UserBehaviourController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;
    private readonly IUserBehaviourService _userBehaviourService;

    public UserBehaviourController(ILogger<UserBehaviourController> logger,
        IDbContextTransactionHandler transactionHandler, IUserBehaviourService userBehaviourService)
    {
        _logger = logger;
        _transactionHandler = transactionHandler;
        _userBehaviourService = userBehaviourService;
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("tutor-profile/{userId:int}")]
    public async Task<IActionResult> TutorProfileVisited(int userId)
    {
        await _transactionHandler.WithTransaction(
            () => _userBehaviourService.TutorProfileVisited(userId, User)
        );

        return NoContent();
    }
}