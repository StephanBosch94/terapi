using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTranslationLanguageRepository : ICTranslationLanguageRepository
{
    private readonly RepositoryContext _db;

    public CTranslationLanguageRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<int> GetTranslationLanguageId(string languageCode, bool throwIfNotFound = true)
    {
        var languageId = await Query()
            .Where(x => x.LanguageCode == languageCode)
            .Select(x => x.TranslationLanguageId)
            .FirstOrDefaultAsync();

        if (languageId == default && throwIfNotFound)
        {
            throw new NotFoundException($"Could not find a language with @code = {languageCode}");
        }

        return languageId;
    }

    private IQueryable<CTranslationLanguage> Query(bool asNoTracking = true)
    {
        var query = _db.CTranslationLanguages;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}