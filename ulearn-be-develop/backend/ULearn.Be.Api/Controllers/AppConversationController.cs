using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/conversation")]
public class AppConversationController : ControllerBase
{
    private readonly IAppConversationService _conversationService;
    private readonly ILogger<AppConversationController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppConversationController(
        IAppConversationService conversationService,
        ILogger<AppConversationController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _conversationService = conversationService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpGet]
    public Task<IReadOnlyCollection<AppConversationVM>> GetAll()
    {
        return _conversationService.GetAll(User);
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost]
    public Task<AppConversationCreateVM> Create(AppConversationInputVM inputVm)
    {
        return _transactionHandler.WithTransaction(() => _conversationService.Create(inputVm, User));
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPut("{conversationId:int}")]
    public async Task<IActionResult> SetRead(int conversationId)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _conversationService.SetRead(conversationId, User)
        );

        return Ok();
    }
}