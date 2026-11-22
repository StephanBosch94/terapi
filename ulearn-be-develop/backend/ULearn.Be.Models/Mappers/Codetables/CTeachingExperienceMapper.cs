using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CTeachingExperienceMapper
{
    public static readonly Expression<Func<CTeachingExperience, CTeachingExperienceVM>> AsVM =
        dataItem => new CTeachingExperienceVM
        {
            TeachingExperienceId = dataItem.TeachingExperienceId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static CTeachingExperienceVM ToVm(CTeachingExperience dataItem)
    {
        return new CTeachingExperienceVM
        {
            TeachingExperienceId = dataItem.TeachingExperienceId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }

    public static IQueryable<CTeachingExperienceVM> ToApiList(this IQueryable<CTeachingExperience> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}