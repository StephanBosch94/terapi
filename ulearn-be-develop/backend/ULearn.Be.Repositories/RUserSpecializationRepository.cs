using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.AppUser.Specialization;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserSpecializationRepository : IRUserSpecializationRepository
{
    private readonly RepositoryContext _db;

    public RUserSpecializationRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AppUserSpecializationVM>> GetByUserId(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.SpecializationId)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ILookup<int, AppUserSpecializationVM>> GetSpecializationsByUserId(IEnumerable<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .OrderBy(x => x.SpecializationId)
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.UserId);
    }

    private IQueryable<RUserSpecialization> Query(bool asNoTracking = true)
    {
        var query = _db.RUserSpecializations;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}