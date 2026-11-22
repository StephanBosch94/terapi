namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserActivatedCouponRepository
{
    Task<bool> Exists(int couponTypeId, int userId);
    Task Add(int userId, IEnumerable<int> couponIds);
}