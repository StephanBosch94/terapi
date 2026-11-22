using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppCoupon;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppCouponRepository
{
    Task<AppCouponVM> GetInfo(string code, int userId);
    Task<AppCoupon> GetRawDbEntity(int couponId);
    Task<IReadOnlyCollection<int>> AddRange(IEnumerable<AppCouponIM> input);
    Task<AppCouponVM> GetDefaultCoupon();
}