using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CSpecializationSeeder
{
    private static readonly string Prefix = CCodetablePrefixesEnum.CSpecialization.GetEnumKey();

    public static readonly List<CSpecialization> Entities = new List<TmpCSpecialization>()
    {
        new()
        {
            Id = 1, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Popular,
            Key = "english_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
        new()
        {
            Id = 2, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_english", Icon = IconConstants.Career
        },
        new()
        {
            Id = 3, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_english", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 4, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Popular,
            Key = "english_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 5, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Popular,
            Key = "english_from_zero", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 6, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Other,
            Key = "english_for_travelling", Icon = IconConstants.Airplane
        },
        new()
        {
            Id = 7, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Other,
            Key = "english_for_high_school_students", Icon = IconConstants.Education
        },
        new()
        {
            Id = 8, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "toeic_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 9, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "toefl_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 10, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "ielts_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 11, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "cambridge_ket_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 12, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "cambridge_pet_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 13, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "cambridge_fce_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 14, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "cambridge_cae_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 15, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Tests,
            Key = "cambridge_cpe_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 16, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Accent,
            Key = "british_accent", Icon = IconConstants.BritishFlag
        },
        new()
        {
            Id = 17, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Accent,
            Key = "american_accent", Icon = IconConstants.UsaFlag
        },
        new()
        {
            Id = 18, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Accent,
            Key = "australian_accent", Icon = IconConstants.AustraliaFlag
        },
        new()
        {
            Id = 19, Subject = CSubjectEnum.English, Category = CSpecializationCategoryEnum.Accent,
            Key = "new_zealand_accent", Icon = IconConstants.NewZealandFlag
        },
        new()
        {
            Id = 20, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Popular,
            Key = "czech_for_foreigners", Icon = IconConstants.PassportControl
        },
        new()
        {
            Id = 21, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Popular,
            Key = "czech_for_beginners", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 22, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_czech", Icon = IconConstants.Career
        },
        new()
        {
            Id = 23, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_czech", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 24, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Popular,
            Key = "czech_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 25, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Popular,
            Key = "czech_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
        new()
        {
            Id = 26, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_language_exam_for_permanent_residence", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 27, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_language_exam_for_citizenship", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 28, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_state_examination", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 29, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_for_foreigners_a2", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 30, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_for_foreigners_b1", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 31, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_for_foreigners_b2", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 32, Subject = CSubjectEnum.Czech, Category = CSpecializationCategoryEnum.Tests,
            Key = "czech_for_foreigners_c1", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 33, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Popular,
            Key = "spanish_for_beginners", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 34, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_spanish", Icon = IconConstants.Career
        },
        new()
        {
            Id = 35, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_spanish", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 36, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Popular,
            Key = "spanish_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 37, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Popular,
            Key = "spanish_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
        new()
        {
            Id = 38, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Popular,
            Key = "european_spanish", Icon = IconConstants.EuropeFlag
        },
        new()
        {
            Id = 39, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Tests,
            Key = "dele_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 40, Subject = CSubjectEnum.Spanish, Category = CSpecializationCategoryEnum.Tests,
            Key = "siele_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 41, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Popular,
            Key = "german_for_beginners", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 42, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_german", Icon = IconConstants.Career
        },
        new()
        {
            Id = 43, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_german", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 44, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Popular,
            Key = "german_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 45, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Popular,
            Key = "german_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
        new()
        {
            Id = 46, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Tests,
            Key = "dsh_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 47, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Tests,
            Key = "goethe_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 48, Subject = CSubjectEnum.German, Category = CSpecializationCategoryEnum.Tests,
            Key = "testdaf_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 49, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Popular,
            Key = "french_for_beginners", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 50, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_french", Icon = IconConstants.Career
        },
        new()
        {
            Id = 51, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_french", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 52, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Popular,
            Key = "french_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 53, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Popular,
            Key = "french_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
        new()
        {
            Id = 54, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Tests,
            Key = "delf_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 55, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Tests,
            Key = "tfi_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 56, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Tests,
            Key = "dalf_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 57, Subject = CSubjectEnum.French, Category = CSpecializationCategoryEnum.Tests,
            Key = "dfp_preparation", Icon = IconConstants.WritingHand
        },
        new()
        {
            Id = 58, Subject = CSubjectEnum.Italian, Category = CSpecializationCategoryEnum.Popular,
            Key = "italian_for_beginners", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 59, Subject = CSubjectEnum.Italian, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_italian", Icon = IconConstants.Career
        },
        new()
        {
            Id = 60, Subject = CSubjectEnum.Italian, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_italian", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 61, Subject = CSubjectEnum.Italian, Category = CSpecializationCategoryEnum.Popular,
            Key = "italian_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 62, Subject = CSubjectEnum.Italian, Category = CSpecializationCategoryEnum.Popular,
            Key = "italian_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
        new()
        {
            Id = 63, Subject = CSubjectEnum.Korean, Category = CSpecializationCategoryEnum.Popular,
            Key = "korean_for_beginners", Icon = IconConstants.ChartUp
        },
        new()
        {
            Id = 64, Subject = CSubjectEnum.Korean, Category = CSpecializationCategoryEnum.Popular,
            Key = "business_korean", Icon = IconConstants.Career
        },
        new()
        {
            Id = 65, Subject = CSubjectEnum.Korean, Category = CSpecializationCategoryEnum.Popular,
            Key = "conversational_korean", Icon = IconConstants.ConnectPeople
        },
        new()
        {
            Id = 66, Subject = CSubjectEnum.Korean, Category = CSpecializationCategoryEnum.Popular,
            Key = "korean_intensive_course", Icon = IconConstants.WomanSuperhero
        },
        new()
        {
            Id = 67, Subject = CSubjectEnum.Korean, Category = CSpecializationCategoryEnum.Popular,
            Key = "korean_for_children", Icon = IconConstants.Kids, IsLessonForChildrenType = true
        },
    }.Select(x => new CSpecialization
    {
        SpecializationId = x.Id,
        SubjectId = (int)x.Subject,
        SpecializationCategoryId = (int)x.Category,
        EnumKey = x.Key,
        Icon = x.Icon,
        DescriptionKey = $"{Prefix}.{x.Key}.description",
        NameKey = $"{Prefix}.{x.Key}.name",
        HeroTitleKey = $"{Prefix}.{x.Key}.hero.title",
        HeroDescriptionKey = $"{Prefix}.{x.Key}.hero.description",
        PageTitleKey = $"{Prefix}.{x.Key}.page.title",
        PageDescriptionKey = $"{Prefix}.{x.Key}.page.description",
        IsLessonForChildrenType = x.IsLessonForChildrenType
    }).ToList();
}

public record TmpCSpecialization
{
    public int Id { get; set; }
    public CSubjectEnum Subject { get; set; }
    public CSpecializationCategoryEnum Category { get; set; }
    public string Key { get; set; }
    public string Icon { get; set; }
    public bool IsLessonForChildrenType { get; set; }
}