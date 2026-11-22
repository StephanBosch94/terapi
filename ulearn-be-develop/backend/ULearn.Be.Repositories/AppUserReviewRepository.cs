using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.AppUserReview;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserReviewRepository : BaseService, IAppUserReviewRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppUserReviewRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<IEnumerable<AppUserReviewVM>> GetByRevieweeId(int userId)
    {
        return await Query()
            .Where(x => x.RevieweeId == userId)
            .OrderByDescending(x => x.Created)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<IEnumerable<AppUserReviewVM>> GetByReviewerId(int userId)
    {
        return await Query()
            .Where(x => x.ReviewerId == userId)
            .OrderByDescending(x => x.Created)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<int> Add(AppUserReviewIM input, int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var newEntity = new AppUserReview
        {
            RevieweeId = input.UserId,
            ReviewerId = userId,
            Rate = input.Value,
            Comment = input.TextValue,
            Created = now,
            Updated = now
        };
        _db.AppUserReviews.Add(newEntity);

        await _db.SaveChangesAsync();
        return newEntity.UserReviewId;
    }

    public async Task Edit(AppUserReviewUpdateIM input, int userId)
    {
        var dbEntity = await Query(false)
            .Where(x => x.ReviewerId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, dbEntity);

        dbEntity.Rate = input.Value;
        dbEntity.Comment = input.TextValue;
        dbEntity.Updated = _timeUtility.GetUtcNow();

        await _db.SaveChangesAsync();
    }

    public async Task<AppUserReviewForNotificationVM> GetNotificationVmById(int userReviewId)
    {
        var result = await Query()
            .Where(x => x.UserReviewId == userReviewId)
            .ToAppUserReviewForNotificationVmList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userReviewId, result);

        return result;
    }

    private IQueryable<AppUserReview> Query(bool asNoTracking = true)
    {
        var query = _db.AppUserReviews;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}