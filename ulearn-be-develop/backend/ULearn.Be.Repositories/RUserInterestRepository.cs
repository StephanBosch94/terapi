using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserInterestRepository : IRUserInterestRepository
{
    private readonly RepositoryContext _db;

    public RUserInterestRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CInterestVM>> GetByUserId(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.InterestId)
            .Select(x => x.CInterest)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ILookup<int, CInterestVM>> GetInterestByUserId(IEnumerable<int> userIds)
    {
        var results = await Query()
            .OrderBy(x => x.InterestId)
            .Where(x => userIds.Contains(x.UserId))
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.UserId);
    }

    private IQueryable<RUserInterest> Query(bool asNoTracking = true)
    {
        var query = _db.RUserInterests;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}