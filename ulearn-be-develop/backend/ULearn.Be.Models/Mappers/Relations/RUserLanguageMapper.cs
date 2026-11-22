using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppUser.Language;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RUserLanguageMapper
{
    public static readonly Expression<Func<RUserLanguage, AppUserLanguageVM>> AsVM =
        dataItem => new AppUserLanguageVM
        {
            UserId = dataItem.UserId,
            Language = CLanguageMapper.ToVm(dataItem.Language),
            Level = CLanguageLevelMapper.ToVm(dataItem.LanguageLevel)
        };

    public static IQueryable<AppUserLanguageVM> ToApiList(this IQueryable<RUserLanguage> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}