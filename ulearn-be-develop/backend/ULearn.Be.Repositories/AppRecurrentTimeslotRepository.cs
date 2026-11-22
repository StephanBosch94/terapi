using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslot;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppRecurrentTimeslotRepository : BaseService, IAppRecurrentTimeslotRepository
{
    private readonly RepositoryContext _db;

    public AppRecurrentTimeslotRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<ICollection<AppRecurrentTimeslot>> GetAllRaw(int userId, int? isoWeekday = null)
    {
        return await Query()
            .Where(x => x.OwnerId == userId)
            .Where(x => isoWeekday == null || isoWeekday == x.LocalIsoWeekDay)
            .ToListAsync();
    }

    public Task Add(AppRecurrentTimeslotInputVM input, int userId)
    {
        var newEntity = new AppRecurrentTimeslot
        {
            OwnerId = userId,
            LocalIsoWeekDay = input.LocalIsoWeekDay,
            FromLocalMinute = input.FromLocalMinute,
            FromLocalHour = input.FromLocalHour,
            DurationInMinutes = input.DurationInMinutes,
            IanaTimezone = input.IanaTimezone
        };

        _db.AppRecurrentTimeslots.Add(newEntity);
        return _db.SaveChangesAsync();
    }

    public async Task Delete(int recurrentTimeslotId, int userId)
    {
        var entity = await Query()
            .Where(x => x.RecurrentTimeslotId == recurrentTimeslotId)
            .Where(x => x.OwnerId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(recurrentTimeslotId, entity);

        _db.AppRecurrentTimeslots.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<AppRecurrentTimeslot> Get(int recurrentTimeslotId, int userId)
    {
        var result = await Query()
            .Where(x => x.RecurrentTimeslotId == recurrentTimeslotId)
            .Where(x => x.OwnerId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(recurrentTimeslotId, result);

        return result;
    }

    public Task<bool> Exists(int userId)
    {
        return Query()
            .Where(x => x.OwnerId == userId)
            .AnyAsync();
    }

    private IQueryable<AppRecurrentTimeslot> Query(bool asNoTracking = true)
    {
        var query = _db.AppRecurrentTimeslots;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}