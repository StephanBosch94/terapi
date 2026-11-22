using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICTranslationRepository
{
    Task<IReadOnlyDictionary<string, string>> GetTranslationDictionary(
        int? translationLanguageId = (int)CTranslationLanguageEnum.English
    );
    
    Task<int> BatchUpsert(ICollection<AppTranslationIM> translations, int userId);

    Task<string> GetTranslationKey(
        string textValue,
        CTranslationLanguageEnum languageEnum,
        bool throwIfNotFound = true
    );

    Task<IReadOnlyDictionary<string, Dictionary<string, string>>> GetAllTranslationsDictionary();
    Task<IReadOnlyDictionary<string, Dictionary<string, string>>> Filter(FilterView<TranslationsFilter> filter);

    Task<IReadOnlyDictionary<string, string>> GetTranslationByKeyPrefix(int translationLanguageId, string keyPrefix);
}