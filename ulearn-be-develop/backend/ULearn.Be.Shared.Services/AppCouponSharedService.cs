using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Models.ViewModels.AppCoupon;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class AppCouponSharedService : BaseService, IAppCouponSharedService
{
    private readonly IAppCouponRepository _appCouponRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IRUserActivatedCouponRepository _rUserActivatedCouponRepository;
    private readonly ITimeUtility _timeUtility;

    public AppCouponSharedService(
        IAppCouponRepository appCouponRepository,
        IRUserActivatedCouponRepository rUserActivatedCouponRepository,
        ITimeUtility timeUtility,
        IAppReservationRepository appReservationRepository
    )
    {
        _appCouponRepository = appCouponRepository;
        _rUserActivatedCouponRepository = rUserActivatedCouponRepository;
        _timeUtility = timeUtility;
        _appReservationRepository = appReservationRepository;
    }

    public async Task<AppCouponVM> GetInfo(string code, int userId)
    {
        var info = await _appCouponRepository.GetInfo(code, userId);
        ThrowIfNotFound($"Could not find a coupon with code [{code}]", info);
        var alreadyActivated = await _rUserActivatedCouponRepository.Exists(info.CouponTypeId, userId);
        info.ReadyToUse = !alreadyActivated && info.ExpireTime > _timeUtility.GetUtcNow();

        return info;
    }

    public async Task<AppCouponVM?> GetDefaultCoupon(ClaimsPrincipal user)
    {
        var hasReservations = await _appReservationRepository.ExistsAnyBookings(user.GetUserId());
        if (hasReservations)
        {
            return null;
        }

        var result = await _appCouponRepository.GetDefaultCoupon();
        result.ReadyToUse = result.ExpireTime > _timeUtility.GetUtcNow();

        return result;
    }
}