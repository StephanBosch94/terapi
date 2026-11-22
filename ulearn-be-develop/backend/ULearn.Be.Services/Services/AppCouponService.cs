using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Models.ViewModels.AppCoupon;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppCouponService : BaseService, IAppCouponService
{
    private readonly IAppCouponSharedService _appCouponSharedService;

    public AppCouponService(IAppCouponSharedService appCouponSharedService)
    {
        _appCouponSharedService = appCouponSharedService;
    }

    public Task<AppCouponVM> GetInfo(string code, ClaimsPrincipal user)
    {
        return _appCouponSharedService.GetInfo(code, user.GetUserId());
    }

    public Task<AppCouponVM> GetDefaultCoupon(ClaimsPrincipal user)
    {
        return _appCouponSharedService.GetDefaultCoupon(user);
    }
}