using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTranslationKeyRepository : ICTranslationKeyRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public CTranslationKeyRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<int> BatchUpsert(ICollection<AppTranslationIM> translations, int userId)
    {
        var all = await Query()
            .ToListAsync();

        var keys = translations.Select(x => x.TranslationKey).ToHashSet();
        var dbEntities = all.Where(x => keys.Contains(x.KeyName)).ToList();
        var newEntities = translations.Select(
            x => new CTranslationKey
            {
                KeyName = x.TranslationKey,
                Created = _timeUtility.GetUtcNow(),
                CreatorId = userId,
                TranslationNamespaceId = (int)CTranslationNamespaceEnum.Default
            }
        ).ToList();

        var compareResultVm = CompareUtil.Compare(newEntities, dbEntities, x => x.KeyName);
        _db.RemoveRange(compareResultVm.ToRemove);
        _db.AddRange(compareResultVm.ToAdd);
        return await _db.SaveChangesAsync();
    }

    private IQueryable<CTranslationKey> Query(bool asNoTracking = true)
    {
        var query = _db.CTranslationKeys;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}