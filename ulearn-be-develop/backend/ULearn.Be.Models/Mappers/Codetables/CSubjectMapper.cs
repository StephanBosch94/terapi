using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CSubjectMapper
{
    public static readonly Expression<Func<CSubject, CSubjectVM>> AsVM =
        dataItem => new CSubjectVM
        {
            SubjectId = dataItem.SubjectId,
            IconName = dataItem.IconName,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            HeroTitleKey = dataItem.HeroTitleKey,
            HeroTitleDescriptionKey = dataItem.HeroDescriptionKey,
            PageTitleKey = dataItem.PageTitleKey,
            TutorPublicPageTitleKey = dataItem.TutorPublicPageTitleKey,
            TutorPublicPageDescriptionKey = dataItem.TutorPublicPageDescriptionKey,
            FooterSubjectLandingPageLinkKey = dataItem.FooterSubjectLandingPageLinkKey,
            FooterSubjectTeachersPageLinkKey = dataItem.FooterSubjectTeachersPageLinkKey,
            UserGoalTitleKey = dataItem.UserGoalTitleKey
        };

    public static CSubjectVM ToVm(CSubject dataItem)
    {
        return new CSubjectVM
        {
            SubjectId = dataItem.SubjectId,
            IconName = dataItem.IconName,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            HeroTitleKey = dataItem.HeroTitleKey,
            HeroTitleDescriptionKey = dataItem.HeroDescriptionKey,
            PageTitleKey = dataItem.PageTitleKey,
            TutorPublicPageTitleKey = dataItem.TutorPublicPageTitleKey,
            TutorPublicPageDescriptionKey = dataItem.TutorPublicPageDescriptionKey,
            FooterSubjectLandingPageLinkKey = dataItem.FooterSubjectLandingPageLinkKey,
            FooterSubjectTeachersPageLinkKey = dataItem.FooterSubjectTeachersPageLinkKey,
            UserGoalTitleKey = dataItem.UserGoalTitleKey
        };
    }

    public static IQueryable<CSubjectVM> ToApiList(this IQueryable<CSubject> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}