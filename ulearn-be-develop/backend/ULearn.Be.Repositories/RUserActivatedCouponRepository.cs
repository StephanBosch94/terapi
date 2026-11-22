using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserActivatedCouponRepository : IRUserActivatedCouponRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public RUserActivatedCouponRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task<bool> Exists(int couponTypeId, int userId)
    {
        return Query()
            .Where(x => x.AppCoupon.CouponTypeId == couponTypeId)
            .Where(x => x.UserId == userId)
            .AnyAsync();
    }

    public Task Add(int userId, IEnumerable<int> couponIds)
    {
        var entities = couponIds
            .Select(x => new RUserActivatedCoupon
            {
                UserId = userId,
                CouponId = x,
                Activated = _timeUtility.GetUtcNow()
            });

        _db.RUserActivatedCoupons.AddRange(entities);

        return _db.SaveChangesAsync();
    }

    private IQueryable<RUserActivatedCoupon> Query(bool asNoTracking = true)
    {
        var query = _db.RUserActivatedCoupons;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}