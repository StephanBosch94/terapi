using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserSubjectLevelRepository : IRUserSubjectLevelRepository
{
    private readonly RepositoryContext _db;

    public RUserSubjectLevelRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CSubjectLevelVM>> GetByUserId(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.SubjectLevelId)
            .Select(x => x.CSubjectLevel)
            .OrderByDescending(x => x.SubjectLevelId)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ILookup<int, CSubjectLevelVM>> GetSubjectLevelsByUserId(IEnumerable<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .OrderByDescending(x => x.SubjectLevelId)
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.UserId);
    }

    private IQueryable<RUserSubjectLevel> Query(bool asNoTracking = true)
    {
        var query = _db.RUserSubjectLevels;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}