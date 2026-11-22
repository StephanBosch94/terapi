using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppRecurrentTimeslotBlacklistRepository : IAppRecurrentTimeslotBlacklistRepository
{
    private readonly RepositoryContext _db;

    public AppRecurrentTimeslotBlacklistRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<ICollection<AppRecurrentTimeslotBlacklist>> GetAllRaw(int userId)
    {
        return await Query()
            .Where(x => x.AppRecurrentTimeslot.OwnerId == userId)
            .ToListAsync();
    }

    public async Task Add(int recurrentTimeslotId, int localYear, int localMonth, int localDay)
    {
        var entity = new AppRecurrentTimeslotBlacklist
        {
            RecurrentTimeslotId = recurrentTimeslotId,
            LocalYear = localYear,
            LocalMonth = localMonth,
            LocalDay = localDay
        };

        _db.AppRecurrentTimeslotBlacklists.Add(entity);
        await _db.SaveChangesAsync();
        _db.Entry(entity).State = EntityState.Detached;
    }

    public Task DeleteByRecurrentTimeslotId(int recurrentTimeslotId)
    {
        var all = Query()
            .Where(x => x.RecurrentTimeslotId == recurrentTimeslotId);
        
        _db.AppRecurrentTimeslotBlacklists.RemoveRange(all);

        return _db.SaveChangesAsync();
    }

    public Task Add(AppRecurrentTimeslot appRecurrentTimeslot, int localYear, int localMonth, int localDay)
    {
        var entity = new AppRecurrentTimeslotBlacklist
        {
            RecurrentTimeslotId = appRecurrentTimeslot.RecurrentTimeslotId,
            LocalYear = localYear,
            LocalMonth = localMonth,
            LocalDay = localDay
        };
        _db.AppRecurrentTimeslotBlacklists.Add(entity);

        return _db.SaveChangesAsync();
    }

    private IQueryable<AppRecurrentTimeslotBlacklist> Query(bool asNoTracking = true)
    {
        var query = _db.AppRecurrentTimeslotBlacklists;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}