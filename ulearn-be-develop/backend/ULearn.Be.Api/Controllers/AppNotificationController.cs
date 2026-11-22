using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Models.ViewModels.Notification;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/notification")]
public class AppNotificationController : ControllerBase
{
    private readonly IAppNotificationService _appNotificationService;
    private readonly ILogger<AppNotificationController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppNotificationController(
        IAppNotificationService appNotificationService,
        ILogger<AppNotificationController> logger,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appNotificationService = appNotificationService;
        _logger = logger;
        _transactionHandler = transactionHandler;
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("filter")]
    [ProducesResponseType(typeof(PagedVM<AppNotificationVM>), StatusCodes.Status200OK)]
    public Task<PagedVM<AppNotificationVM>> Filter(FilterView<AppNotificationFilterVM> filter)
    {
        return _appNotificationService.Filter(filter, User);
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPut("{notificationId:int}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Read(int notificationId)
    {
        await _transactionHandler.WithTransaction(() =>
            _appNotificationService.Read(notificationId, User)
        );

        return NoContent();
    }

    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPut("read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadAll()
    {
        await _transactionHandler.WithTransaction(() =>
            _appNotificationService.ReadAll(User)
        );

        return NoContent();
    }
    
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPut("settings")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EditSettings(IReadOnlyCollection<RUserNotificationSettingIM> input)
    {
        await _transactionHandler.WithTransaction(() =>
            _appNotificationService.EditSettings(input, User)
        );

        return NoContent();
    }
    
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpGet("settings")]
    [ProducesResponseType(typeof(IReadOnlyCollection<RUserNotificationSettingVM>), StatusCodes.Status200OK)]
    public Task<IReadOnlyCollection<RUserNotificationSettingVM>> GetSettings()
    {
        return _appNotificationService.GetSettings(User);
    }
}