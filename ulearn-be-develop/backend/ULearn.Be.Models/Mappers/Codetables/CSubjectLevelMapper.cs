using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CSubjectLevelMapper
{
    public static readonly Expression<Func<CSubjectLevel, CSubjectLevelVM>> AsVM =
        dataItem => new CSubjectLevelVM
        {
            SubjectLevelId = dataItem.SubjectLevelId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            TutorNameKey = dataItem.TutorNameKey,
            StudentNameKey = dataItem.StudentNameKey,
            IconName = dataItem.IconName,
            InvertedIconName = dataItem.InvertedIconName
        };

    public static IQueryable<CSubjectLevelVM> ToApiList(this IQueryable<CSubjectLevel> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}