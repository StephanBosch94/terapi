using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppCoupon;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppCouponRepository : IAppCouponRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppCouponRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task<AppCouponVM> GetInfo(string code, int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return Query()
            .Where(x => x.ValidFrom <= now)
            .Where(x => x.Code == code)
            .Where(x => x.UserId == null || x.UserId == userId)
            .ToApiList()
            .FirstOrDefaultAsync();
    }

    public Task<AppCoupon> GetRawDbEntity(int couponId)
    {
        return Query()
            .Where(x => x.CouponId == couponId)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<int>> AddRange(IEnumerable<AppCouponIM> input)
    {
        var now = _timeUtility.GetUtcNow();
        var nextDay = now.Date.AddDays(1);
        var entities = input.Select(x => new AppCoupon
        {
            UserId = x.UserId,
            Code = x.Code,
            Created = now,
            ValidFrom = now.Date,
            ValidTo = nextDay.AddDays(x.ExpireTimeInDays),
            DiscountPercentage = x.DiscountPercentage,
            CouponTypeId = (int)x.CouponTypeEnum
        }).ToList();

        _db.AddRange(entities);
        await _db.SaveChangesAsync();
        return entities.Select(x => x.CouponId).ToHashSet();
    }

    public async Task<AppCouponVM> GetDefaultCoupon()
    {
        var now = _timeUtility.GetUtcNow();
        var result = await Query()
            .Where(x => x.ValidFrom <= now)
            .Where(x => x.UserId == null)
            .OrderByDescending(x => x.CouponId)
            .ToApiList()
            .FirstOrDefaultAsync();

        if (result == null)
        {
            throw new Exception("Default coupon has not been configured");
        }

        return result;
    }

    private IQueryable<AppCoupon> Query(bool asNoTracking = true)
    {
        var query = _db.AppCoupons;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}