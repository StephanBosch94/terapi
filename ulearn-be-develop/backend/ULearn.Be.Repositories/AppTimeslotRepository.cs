using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppTimeslot;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppTimeslotRepository : BaseService, IAppTimeslotRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppTimeslotRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<ICollection<AppTimeslot>> GetAllRaw(int userId)
    {
        return await Query()
            .Where(x => x.OwnerId == userId)
            .ToListAsync();
    }

    public async Task Add(AppTimeslotInputVM input, int userId)
    {
        var fromTime = new DateTime(input.FromLocalYear, input.FromLocalMonth, input.FromLocalDay)
            .ToUniversalTime()
            .AddHours(input.FromLocalHour)
            .AddMinutes(input.FromLocalMinute);

        var newEntity = new AppTimeslot
        {
            OwnerId = userId,
            FromTime = _timeUtility.ConvertFromLocalToUtcTime(input.IanaTimezone, fromTime),
            FromLocalYear = input.FromLocalYear,
            FromLocalMonth = input.FromLocalMonth,
            FromLocalDay = input.FromLocalDay,
            FromLocalHour = input.FromLocalHour,
            FromLocalMinute = input.FromLocalMinute,
            DurationInMinutes = input.DurationInMinutes,
            IanaTimezone = input.IanaTimezone
        };

        _db.AppTimeslots.Add(newEntity);
        await _db.SaveChangesAsync();
        _db.Entry(newEntity).State = EntityState.Detached;
    }

    public async Task Delete(int timeslotId, int userId)
    {
        var timeslot = await Query()
            .Where(x => x.TimeslotId == timeslotId)
            .Where(x => x.OwnerId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(timeslotId, timeslot);
        _db.AppTimeslots.Remove(timeslot);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<AppTimeslotBaseVM>> GetBaseVMs(int userId)
    {
        return await Query()
            .Where(x => x.OwnerId == userId)
            .ToBaseVMs()
            .ToListAsync();
    }

    public Task CreateRange(IEnumerable<AppTimeslot> appTimeslots)
    {
        _db.AppTimeslots.AddRange(appTimeslots);
        return _db.SaveChangesAsync();
    }

    public Task<bool> Exists(int userId)
    {
        return Query()
            .Where(x => x.OwnerId == userId)
            .AnyAsync();
    }

    private IQueryable<AppTimeslot> Query(bool asNoTracking = true)
    {
        var query = _db.AppTimeslots;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}