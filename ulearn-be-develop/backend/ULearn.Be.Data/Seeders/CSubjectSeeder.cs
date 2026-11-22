using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CSubjectSeeder
{
    private const CCodetablePrefixesEnum Pefix = CCodetablePrefixesEnum.CSubject;

    private static readonly List<CTmpSubject> TmpSubjects = new()
    {
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.English,
            IconName = IconConstants.BritishFlag
        },
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.Czech,
            IconName = IconConstants.CzechFlag
        },
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.French,
            IconName = IconConstants.FrenchFlag
        },
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.Spanish,
            IconName = IconConstants.SpanishFlag
        },
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.German,
            IconName = IconConstants.GermanFlag
        },
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.Italian,
            IconName = IconConstants.ItalianFlag
        },
        new CTmpSubject
        {
            CategoryEnum = CCategoryEnum.Languages,
            SubjectEnum = CSubjectEnum.Korean,
            IconName = IconConstants.KoreanFlag
        }
    };

    public static readonly List<CSubject> Entities = TmpSubjects.Select(x =>
        new CSubject
        {
            CategoryId = (int)x.CategoryEnum,
            SubjectId = (int)x.SubjectEnum,
            IconName = x.IconName,
            EnumKey = x.SubjectEnum.GetEnumKey(),
            NameKey = Pefix.GetNameKey(x.SubjectEnum.GetEnumKey()),
            DescriptionKey = Pefix.GetDescriptionKey(x.SubjectEnum.GetEnumKey()),
            HeroTitleKey = $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.hero.title",
            HeroDescriptionKey = $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.hero.description",
            PageTitleKey = $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.page.title",
            TutorPublicPageTitleKey = $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.tutor_public_page.title",
            TutorPublicPageDescriptionKey = $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.tutor_public_page.description",
            FooterSubjectLandingPageLinkKey =
                $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.footer.lading_page_link",
            FooterSubjectTeachersPageLinkKey =
                $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.footer.subject_teachers_link",
            UserGoalTitleKey = $"{Pefix.GetEnumKey()}.{x.SubjectEnum.GetEnumKey()}.user_goal.title",
            ObjectStateId = (int)CObjectStateEnum.Active
        }).ToList();
}

public record CTmpSubject
{
    public CCategoryEnum CategoryEnum { get; set; }
    public CSubjectEnum SubjectEnum { get; set; }
    public string IconName { get; set; }
}; 