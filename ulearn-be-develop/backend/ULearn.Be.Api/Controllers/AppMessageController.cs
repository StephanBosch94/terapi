using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/message")]
public class AppMessageController : ControllerBase
{
    private readonly ILogger<AppMessageController> _logger;
    private readonly IAppMessageService _messageService;

    public AppMessageController(IAppMessageService messageService, ILogger<AppMessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [RequestSizeLimit(209_715_200)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromForm] string messageText,
        [FromForm] [Required] int opponentId
    )
    {
        await _messageService.Create(opponentId, messageText, Request.Form.Files, User);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("conversation/{conversationId:int}/filter")]
    public Task<PagedVM<AppMessageVM>> Filter(int conversationId, FilterView<MessageFilterVM> filter)
    {
        return _messageService.Filter(conversationId, filter, User);
    }
}