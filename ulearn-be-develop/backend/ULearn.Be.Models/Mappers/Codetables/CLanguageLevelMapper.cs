using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CLanguageLevelMapper
{
    public static readonly Expression<Func<CLanguageLevel, CLanguageLevelVM>> AsVM =
        dataItem => new CLanguageLevelVM
        {
            LanguageLevelId = dataItem.LanguageLevelId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static CLanguageLevelVM ToVm(CLanguageLevel dataItem)
    {
        return new CLanguageLevelVM
        {
            LanguageLevelId = dataItem.LanguageLevelId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }

    public static IQueryable<CLanguageLevelVM> ToApiList(this IQueryable<CLanguageLevel> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}