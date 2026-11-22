using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserReservationReviewRepository : BaseService, IAppUserReservationReviewRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppUserReservationReviewRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task Add(AppReservationReviewIM input, int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var newEntity = new AppReservationReview
        {
            ReservationId = input.ReservationId,
            CreatorId = userId,
            Value = input.Value,
            TextValue = input.TextValue,
            Created = now,
            Updated = now
        };

        _db.AppReservationReviews.Add(newEntity);
        return _db.SaveChangesAsync();
    }

    public async Task<ILookup<int, AppReservationReviewVM>> GetReviewsByReservationId(IEnumerable<int> reservationIds)
    {
        var results = await Query()
            .Where(x => reservationIds.Contains(x.ReservationId))
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.ReservationId);
    }

    public async Task Edit(AppReservationReviewEditIM input, int userId)
    {
        var dbEntity = await Query(false)
            .Where(x => x.ReservationReviewId == input.ReservationReviewId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(input.ReservationReviewId, dbEntity);

        dbEntity.Updated = _timeUtility.GetUtcNow();
        dbEntity.TextValue = input.TextValue;
        dbEntity.Value = input.Value;

        await _db.SaveChangesAsync();
    }

    private IQueryable<AppReservationReview> Query(bool asNoTracking = true)
    {
        var query = _db.AppReservationReviews;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}