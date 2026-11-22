namespace ULearn.Be.Models.ViewModels.AppCoupon;

public class AppCouponVM
{
    public int CouponId { get; set; }
    public int CouponTypeId { get; set; }
    public int DiscountPercentage { get; set; }
    public DateTime Created { get; set; }
    public DateTime ExpireTime { get; set; }
    public string Code { get; set; }
    public bool ReadyToUse { get; set; }
}