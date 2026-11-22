using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class TranslationService : ITranslationService
{
    private readonly ICTranslationLanguageRepository _cTranslationLanguageRepository;
    private readonly IHashUtility _hashUtility;
    private readonly IMasterHashRepository _masterHashRepository;
    private readonly ICTranslationKeyRepository _translationKeyRepository;
    private readonly ICTranslationRepository _translationRepository;


    public TranslationService(
        ICTranslationRepository translationRepository,
        ICTranslationKeyRepository translationKeyRepository,
        IMasterHashRepository masterHashRepository,
        ICTranslationLanguageRepository cTranslationLanguageRepository,
        IHashUtility hashUtility
    )
    {
        _translationRepository = translationRepository;
        _translationKeyRepository = translationKeyRepository;
        _masterHashRepository = masterHashRepository;
        _cTranslationLanguageRepository = cTranslationLanguageRepository;
        _hashUtility = hashUtility;
    }

    public Task<IReadOnlyDictionary<string, string>> GetTranslationDictionary(int translationLanguageId)
    {
        return _translationRepository.GetTranslationDictionary(translationLanguageId);
    }

    public async Task<IReadOnlyDictionary<string, string>> GetTranslationDictionary(string languageCode)
    {
        var languageId = await _cTranslationLanguageRepository.GetTranslationLanguageId(languageCode);
        return await _translationRepository.GetTranslationDictionary(languageId);
    }

    public async Task BatchUpsert(ICollection<AppTranslationIM> translations, ClaimsPrincipal user)
    {
        var updatedKeys = await _translationKeyRepository.BatchUpsert(translations, user.GetUserId());
        var updatedTranslations = await _translationRepository.BatchUpsert(translations, user.GetUserId());
        if (updatedKeys > 0 || updatedTranslations > 0)
        {
            var newGuid = _hashUtility.GenerateGuid();
            var masterHashes =
                await _masterHashRepository.GetByEnumKeyPrefix(CMasterHashEnum.CTranslation.GetEnumKey());
            await _masterHashRepository.Update(masterHashes.Select(x => x.EnumKey).ToHashSet(), newGuid);
        }
    }

    public Task<IReadOnlyDictionary<string, Dictionary<string, string>>> FilterTranslations(
        FilterView<TranslationsFilter> filter
    )
    {
        return _translationRepository.Filter(filter);
    }

    public Task<IReadOnlyDictionary<string, Dictionary<string, string>>> GetTranslationDictionary()
    {
        return _translationRepository.GetAllTranslationsDictionary();
    }
}