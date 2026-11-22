using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppCoupon;

namespace ULearn.Be.Models.Mappers;

public static class AppCouponMapper
{
    private static readonly Expression<Func<AppCoupon, AppCouponVM>> AsVM =
        dataItem => new AppCouponVM
        {
            CouponId = dataItem.CouponId,
            CouponTypeId = dataItem.CouponTypeId,
            DiscountPercentage = dataItem.DiscountPercentage,
            Created = dataItem.Created,
            ExpireTime = dataItem.ValidTo,
            Code = dataItem.Code
        };

    public static IQueryable<AppCouponVM> ToApiList(this IQueryable<AppCoupon> dataCollection)
    {
        return dataCollection.Select(AsVM);
    }
}