using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CLanguageMapper
{
    public static readonly Expression<Func<CLanguage, CLanguageVM>> AsVM =
        dataItem => new CLanguageVM
        {
            LanguageId = dataItem.LanguageId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static CLanguageVM ToVm(CLanguage dataItem)
    {
        return new CLanguageVM
        {
            LanguageId = dataItem.LanguageId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }

    public static IQueryable<CLanguageVM> ToApiList(this IQueryable<CLanguage> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}