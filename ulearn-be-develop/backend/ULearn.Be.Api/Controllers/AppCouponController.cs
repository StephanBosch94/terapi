using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Models.ViewModels.AppCoupon;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/coupon")]
public class AppCouponController : ControllerBase
{
    private readonly IAppCouponService _appCouponService;
    private readonly ILogger<AppCouponController> _logger;

    public AppCouponController(IAppCouponService appCouponService, ILogger<AppCouponController> logger)
    {
        _appCouponService = appCouponService;
        _logger = logger;
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    [HttpGet("code/{code}")]
    [ProducesResponseType(typeof(AppCouponVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<AppCouponVM> GetInfo(string code)
    {
        return _appCouponService.GetInfo(code, User);
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    [HttpGet("default")]
    [ProducesResponseType(typeof(AppCouponVM), StatusCodes.Status200OK)]
    public Task<AppCouponVM> GetDefaultCoupon()
    {
        return _appCouponService.GetDefaultCoupon(User);
    }
}