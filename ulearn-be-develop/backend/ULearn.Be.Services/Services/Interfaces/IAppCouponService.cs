using System.Security.Claims;
using ULearn.Be.Models.ViewModels.AppCoupon;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppCouponService
{
    Task<AppCouponVM> GetInfo(string code, ClaimsPrincipal user);
    Task<AppCouponVM> GetDefaultCoupon(ClaimsPrincipal user);
}