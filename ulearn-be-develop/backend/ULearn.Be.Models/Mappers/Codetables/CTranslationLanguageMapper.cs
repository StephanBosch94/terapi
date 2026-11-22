using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CTranslationLanguageMapper
{
    public static readonly Expression<Func<CTranslationLanguage, CTranslationLanguageVM>> AsVM =
        dataItem => new CTranslationLanguageVM
        {
            TranslationLanguageId = dataItem.TranslationLanguageId,
            LanguageCode = dataItem.LanguageCode,
            Language = dataItem.Language
        };


    public static IQueryable<CTranslationLanguageVM> ToApiList(this IQueryable<CTranslationLanguage> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}