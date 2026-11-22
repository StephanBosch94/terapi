using System.Security.Claims;
using ULearn.Be.Models.ViewModels.AppCoupon;

namespace ULearn.Be.Shared.Services.Interfaces;

public interface IAppCouponSharedService
{
    Task<AppCouponVM> GetInfo(string code, int userId);
    Task<AppCouponVM?> GetDefaultCoupon(ClaimsPrincipal user);
}