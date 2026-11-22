using Quartz;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.AppCoupon;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace Yaspeak.Be.Scheduler.Jobs;

public class AppCouponScheduledJob : IJob
{
    private const int CouponExpireTimeInDays = 14;
    public static readonly JobKey Key = new("app-coupon-job");
    private readonly IAppCouponRepository _appCouponRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IGuidUtility _guidUtility;
    private readonly INotificationSharedService _notificationSharedService;

    private readonly List<CouponScheduleSetting> _settings = new()
    {
        new CouponScheduleSetting
        {
            DiscountPercentage = 10,
            DurationInDays = 2
        },
        new CouponScheduleSetting
        {
            DiscountPercentage = 15,
            DurationInDays = 7
        },
        new CouponScheduleSetting
        {
            DiscountPercentage = 18,
            DurationInDays = 14
        },
        new CouponScheduleSetting
        {
            DiscountPercentage = 21,
            DurationInDays = 21
        }
    };

    public AppCouponScheduledJob(
        IAppUserRepository appUserRepository,
        IAppCouponRepository appCouponRepository,
        IGuidUtility guidUtility,
        INotificationSharedService notificationSharedService
    )
    {
        _appUserRepository = appUserRepository;
        _appCouponRepository = appCouponRepository;
        _guidUtility = guidUtility;
        _notificationSharedService = notificationSharedService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        const CCouponTypeEnum couponTypeEnum = CCouponTypeEnum.OneLessonSale;
        foreach (var setting in _settings)
        {
            var studentIdsToSend = await _appUserRepository.GetDiscountCouponUserIdsToSend(
                couponTypeEnum,
                setting.DiscountPercentage,
                setting.DurationInDays
            ) ?? new List<int>();

            var couponIds = await _appCouponRepository.AddRange(
                studentIdsToSend.Select(userId => new AppCouponIM
                {
                    UserId = userId,
                    ExpireTimeInDays = CouponExpireTimeInDays,
                    DiscountPercentage = setting.DiscountPercentage,
                    CouponTypeEnum = couponTypeEnum,
                    Code = _guidUtility.GenerateShortCode()
                })) ?? new List<int>();

            foreach (var couponId in couponIds)
            {
                await _notificationSharedService.SendPersonalCoupon(couponId);
            }

            await _appUserRepository.UpdateCouponLastSendTime(studentIdsToSend);
        }
    }

    private record CouponScheduleSetting
    {
        public int DiscountPercentage { get; set; }
        public int DurationInDays { get; set; }
    }
}