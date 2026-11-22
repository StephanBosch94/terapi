using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.AppUser.Language;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserLanguageRepository : IRUserLanguageRepository
{
    private readonly RepositoryContext _db;

    public RUserLanguageRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AppUserLanguageVM>> GetByUserId(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.LanguageLevelId)
            .ThenByDescending(x => x.LanguageId)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ILookup<int, AppUserLanguageVM>> GetLanguagesByUserId(IEnumerable<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .OrderByDescending(x => x.LanguageLevelId)
            .ThenByDescending(x => x.LanguageId)
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.UserId);
    }

    private IQueryable<RUserLanguage> Query(bool asNoTracking = true)
    {
        var query = _db.RUserLanguages;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}