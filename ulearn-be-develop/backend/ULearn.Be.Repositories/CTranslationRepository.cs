using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTranslationRepository : ICTranslationRepository
{
    private readonly RepositoryContext _db;
    private readonly ILogger<CTranslationRepository> _logger;
    private readonly ITimeUtility _timeUtility;

    public CTranslationRepository(RepositoryContext db, ITimeUtility timeUtility,
        ILogger<CTranslationRepository> logger)
    {
        _db = db;
        _timeUtility = timeUtility;
        _logger = logger;
    }

    public async Task<IReadOnlyDictionary<string, Dictionary<string, string>>> GetAllTranslationsDictionary()
    {
        var data = await Query()
            .Select(x => new
            {
                x.CTranslationLanguage.LanguageCode,
                x.TranslationKey.KeyName,
                x.TextValue
            }).ToListAsync();
        return data.GroupBy(x => x.LanguageCode)
            .ToDictionary(
                x => x.Key,
                y => y.ToDictionary(z => z.KeyName, z => z.TextValue)
            );
    }

    public async Task<IReadOnlyDictionary<string, Dictionary<string, string>>> Filter(
        FilterView<TranslationsFilter> filter)
    {
        var languageCodes = filter.Filter?.LanguageCodes?.ToHashSet() ?? new HashSet<string>();

        var data = await Query()
            .Where(x => languageCodes.Contains(x.CTranslationLanguage.LanguageCode))
            .Select(x => new
            {
                x.CTranslationLanguage.LanguageCode,
                x.TranslationKey.KeyName,
                x.TextValue
            })
            .ToListAsync();

        return data
            .GroupBy(x => x.LanguageCode)
            .ToDictionary(
                x => x.Key,
                y => y.ToDictionary(
                    z => z.KeyName,
                    z => z.TextValue
                )
            );
    }

    public async Task<IReadOnlyDictionary<string, string>> GetTranslationDictionary(
        int? translationLanguageId = (int)CTranslationLanguageEnum.English
    )
    {
        return await Query()
            .Include(x => x.TranslationKey)
            .Where(x => x.TranslationLanguageId == translationLanguageId)
            .ToDictionaryAsync(x => x.TranslationKey.KeyName, x => x.TextValue);
    }

    public async Task<int> BatchUpsert(ICollection<AppTranslationIM> translations, int userId)
    {
        var all = await Query(false).Include(x => x.TranslationKey).ToListAsync();
        var cTranslationKeyIds = await _db.CTranslationKeys.ToDictionaryAsync(x => x.KeyName, x => x.KeyId);
        var cTranslationLanguageIds = await _db.CTranslationLanguages.ToDictionaryAsync(
            x => x.LanguageCode,
            x => x.TranslationLanguageId
        );
        var keys = translations.Select(x => x.TranslationKey).ToHashSet();
        var dbEntities = all.Where(x => keys.Contains(x.TranslationKey.KeyName)).ToList();
        var translationLanguageCodes =
            translations.SelectMany(x => x.Values.Select(y => y.LanguageCode)).ToHashSet();
        var missingLanguages = translationLanguageCodes.Except(cTranslationLanguageIds.Keys).ToHashSet();
        if (missingLanguages.Any())
        {
            throw new NotFoundException(ErrorCodesEnum.MissingLanguages, new Dictionary<ErrorCodeParametersEnum, string>
            {
                { ErrorCodeParametersEnum.LanguageCodes, string.Join(",", missingLanguages) }
            });
        }

        var translationKeys = translations.Select(x => x.TranslationKey).ToHashSet();
        var missingKeys = translationKeys.Except(cTranslationKeyIds.Keys).ToHashSet();
        if (missingKeys.Any())
        {
            throw new NotFoundException(ErrorCodesEnum.MissingKeys, new Dictionary<ErrorCodeParametersEnum, string>
            {
                { ErrorCodeParametersEnum.TranslationKeys, string.Join(",", missingKeys) }
            });
        }

        var newEntities = translations.SelectMany(
            x => x.Values.Select(
                y => new CTranslation
                {
                    TranslationKeyId = cTranslationKeyIds[x.TranslationKey],
                    TranslationLanguageId = cTranslationLanguageIds[y.LanguageCode],
                    TextValue = y.Value,
                    IsProtected = true,
                    Created = _timeUtility.GetUtcNow(),
                    CreatorId = userId
                }
            )
        ).ToList();

        var compareResultVm = CompareUtil.Compare(
            newEntities,
            dbEntities,
            x => $"[{x.TranslationLanguageId}][{x.TranslationKeyId}]"
        );
        _db.RemoveRange(compareResultVm.ToRemove);
        _db.AddRange(compareResultVm.ToAdd);
        foreach (var (dbEntity, newerEntity) in compareResultVm.ToUpdate)
        {
            if (dbEntity.TextValue != newerEntity.TextValue)
            {
                dbEntity.TextValue = newerEntity.TextValue;
                dbEntity.UpdaterId = userId;
                dbEntity.Updated = _timeUtility.GetUtcNow();
            }
        }

        return await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyDictionary<string, string>> GetTranslationByKeyPrefix(int translationLanguageId,
        string keyPrefix)
    {
        var result = await Query()
            .Where(x => x.TranslationLanguageId == translationLanguageId)
            .Where(x => x.TranslationKey.KeyName.StartsWith(keyPrefix))
            .Select(x => new
            {
                TranslationKey = x.TranslationKey.KeyName,
                Value = x.TextValue
            })
            .ToListAsync();

        return result.ToDictionary(x => x.TranslationKey, x => x.Value);
    }

    public async Task<string> GetTranslationKey(
        string textValue,
        CTranslationLanguageEnum languageEnum,
        bool throwIfNotFound = true
    )
    {
        var result = await Query()
            .Where(x => x.TranslationLanguageId == (int)languageEnum)
            .Where(x => x.TextValue == textValue)
            .Select(x => x.TranslationKey.KeyName)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            var errorMessage =
                $"Could not find a translation with value {textValue} and language {languageEnum.GetEnumKey()}";
            if (throwIfNotFound)
            {
                throw new NotFoundException(errorMessage);
            }

            _logger.LogError("{ErrorMessage}", errorMessage);
        }

        return result;
    }

    private IQueryable<CTranslation> Query(bool asNoTracking = true)
    {
        var query = _db.CTranslations;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}