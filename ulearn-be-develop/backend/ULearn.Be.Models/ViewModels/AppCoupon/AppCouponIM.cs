using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.AppCoupon;

public class AppCouponIM
{
    public CCouponTypeEnum CouponTypeEnum { get; set; }
    public int UserId { get; set; }
    public int DiscountPercentage { get; set; }
    public int ExpireTimeInDays { get; set; }
    public string Code { get; set; }
}