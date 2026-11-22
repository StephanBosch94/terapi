using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RUserSubjectLevelMapper
{
    public static readonly Expression<Func<RUserSubjectLevel, CSubjectLevelVM>> AsVM =
        dataItem => new CSubjectLevelVM
        {
            UserId = dataItem.UserId,
            SubjectLevelId = dataItem.SubjectLevelId,
            EnumKey = dataItem.CSubjectLevel.EnumKey,
            DescriptionKey = dataItem.CSubjectLevel.DescriptionKey,
            IconName = dataItem.CSubjectLevel.IconName,
            InvertedIconName = dataItem.CSubjectLevel.InvertedIconName,
            StudentNameKey = dataItem.CSubjectLevel.StudentNameKey,
            TutorNameKey = dataItem.CSubjectLevel.TutorNameKey
        };

    public static IQueryable<CSubjectLevelVM> ToApiList(this IQueryable<RUserSubjectLevel> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}