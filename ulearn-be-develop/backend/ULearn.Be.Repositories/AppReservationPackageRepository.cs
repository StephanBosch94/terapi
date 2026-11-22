using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppReservationPackageRepository : BaseService, IAppReservationPackageRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppReservationPackageRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<int> Create(int tutorId, int studentId, double totalPrice, CClassDurationEnum durationEnum)
    {
        var entity = new AppReservationPackage
        {
            HostId = tutorId,
            AttenderId = studentId,
            Created = _timeUtility.GetUtcNow(),
            ReservationPackageStateId = (int)CReservationPackageStateEnum.Created,
            TotalPrice = totalPrice,
            CurrencyId = (int)CCurrencyEnum.Czk,
            ClassDurationId = (int)durationEnum
        };

        _db.AppReservationsPackages.Add(entity);
        await _db.SaveChangesAsync();

        return entity.ReservationPackageId;
    }

    public async Task Cancel(int packageId, string reasonText)
    {
        var entity = await Query()
            .Where(x => x.ReservationPackageId == packageId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(packageId, entity);

        entity.CancellationReason = reasonText;
        entity.Cancelled = _timeUtility.GetUtcNow();
        entity.ReservationPackageStateId = (int)CReservationPackageStateEnum.Cancelled;

        await _db.SaveChangesAsync();
    }

    public Task<double> GetTotalPrice(int reservationPackageId)
    {
        return Query()
            .Where(x => x.ReservationPackageId == reservationPackageId)
            .Select(x => x.TotalPrice)
            .FirstOrDefaultAsync();
    }

    public Task<AppReservationPackage> GetRawDbEntity(int reservationPackageId)
    {
        return Query()
            .Where(x => x.ReservationPackageId == reservationPackageId)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyDictionary<int, AppUserCommonVM>> GetOpponentByPackageId(
        IReadOnlyCollection<int> reservationPackageIds,
        int userId
    )
    {
        var query = Query()
            .Where(x => reservationPackageIds.Contains(x.ReservationPackageId));

        var attendersOpponents = await query
            .Where(x => x.HostId == userId)
            .ToOpponentList()
            .ToListAsync();

        var hostOpponents = await query
            .Where(x => x.AttenderId == userId)
            .ToOpponentList()
            .ToListAsync();

        return attendersOpponents
            .Select(x => new
            {
                x.ReservationPackageId,
                Opponent = x.Attender
            })
            .Concat(
                hostOpponents.Select(x => new
                {
                    x.ReservationPackageId,
                    Opponent = x.Host
                })
            )
            .ToDictionary(x => x.ReservationPackageId, x => x.Opponent);
    }

    private IQueryable<AppReservationPackage> Query(bool asNoTracking = true)
    {
        var query = _db.AppReservationsPackages;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}