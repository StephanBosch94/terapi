using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ITranslationService
{
    Task<IReadOnlyDictionary<string, string>> GetTranslationDictionary(int translationLanguageId);
    Task<IReadOnlyDictionary<string, string>> GetTranslationDictionary(string languageCode);
    Task BatchUpsert(ICollection<AppTranslationIM> translations, ClaimsPrincipal user);

    Task<IReadOnlyDictionary<string, Dictionary<string, string>>> FilterTranslations(
        FilterView<TranslationsFilter> filter
    );
}