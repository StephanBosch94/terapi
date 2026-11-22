using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CSpecializationMapper
{
    public static readonly Expression<Func<CSpecialization, CSpecializationVM>> AsVM =
        dataItem => new CSpecializationVM
        {
            SpecializationId = dataItem.SpecializationId,
            SpecializationCategoryId = dataItem.SpecializationCategoryId,
            SubjectId = dataItem.SubjectId,
            SpecializationCategoryNameKey = dataItem.CSpecializationCategory.NameKey,
            EnumKey = dataItem.EnumKey,
            NameKey = dataItem.NameKey,
            DescriptionKey = dataItem.DescriptionKey,
            IconName = dataItem.Icon,
            HeroTitleKey = dataItem.HeroTitleKey,
            HeroDescriptionKey = dataItem.HeroDescriptionKey,
            PageTitleKey = dataItem.PageTitleKey,
            PageDescriptionKey = dataItem.PageDescriptionKey
        };

    public static IQueryable<CSpecializationVM> ToApiList(this IQueryable<CSpecialization> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}