using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppReservationIssue;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppReservationIssueRepository : BaseRepository, IAppReservationIssueRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppReservationIssueRepository(RepositoryContext db, ITimeUtility timeUtility) : base(db)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<int> Add(ReservationIssueIM input, int userId)
    {
        var entity = new AppReservationIssue
        {
            ReservationId = input.ReservationId,
            CreatorId = userId,
            Created = _timeUtility.GetUtcNow(),
            Description = input.Description,
            ReservationIssueTypeId = (int)input.IssueTypeId,
            ReservationIssueStateId = (int)CReservationIssueStateEnum.Waiting
        };

        _db.AppReservationIssues.Add(entity);

        await _db.SaveChangesAsync();
        return entity.ReservationIssueId;
    }

    public async Task<AppReservationIssue> GetRawDbEntity(int reservationIssueId)
    {
        var result = await Query()
            .Where(x => x.ReservationIssueId == reservationIssueId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(reservationIssueId, result);

        return result;
    }

    public async Task<AppReservationIssueForNotificationVM> GetNotificationVmById(int reservationIssueId)
    {
        var result = await Query()
            .Where(x => x.ReservationIssueId == reservationIssueId)
            .ToAppUserReviewForNotificationVmList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(reservationIssueId, result);

        return result;
    }

    public async Task Resolve(
        int reservationIssueId,
        CReservationIssueStateEnum stateEnum,
        string decisionText,
        int userId
    )
    {
        var entity = await Query(false)
            .Where(x => x.ReservationIssueId == reservationIssueId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(reservationIssueId, entity);

        entity!.ReservationIssueStateId = (int)stateEnum;
        entity.DecisionText = decisionText;
        entity.ResolverId = userId;
        entity.Resolved = _timeUtility.GetUtcNow();

        await _db.SaveChangesAsync();
    }

    public Task<PagedVM<AppReservationIssueVM>> Filter(FilterView<AppReservationIssueFilterVM> filter)
    {
        return Query()
            .ToApiList()
            .ToPagedVMAsync(filter);
    }

    private IQueryable<AppReservationIssue> Query(bool asNoTracking = true)
    {
        var query = _db.AppReservationIssues;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}