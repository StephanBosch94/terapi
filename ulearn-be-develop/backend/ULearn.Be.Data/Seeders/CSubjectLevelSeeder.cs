using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CSubjectLevelSeeder
{
    public static readonly List<CSubjectLevel> Entities = new()
    {
        new CSubjectLevel
        {
            SubjectLevelId = (int)CSubjectLevelEnum.Beginner,
            EnumKey = CSubjectLevelEnum.Beginner.GetEnumKey(),
            TutorNameKey = $"subject.level.{CSubjectLevelEnum.Beginner.GetEnumKey()}.tutor.name",
            StudentNameKey = $"subject.level.{CSubjectLevelEnum.Beginner.GetEnumKey()}.student.name",
            DescriptionKey = $"subject.level.{CSubjectLevelEnum.Beginner.GetEnumKey()}.description",
            IconName = IconConstants.LevelOne,
            InvertedIconName = IconConstants.LevelOneInverted
        },
        new CSubjectLevel
        {
            SubjectLevelId = (int)CSubjectLevelEnum.Intermediate,
            EnumKey = CSubjectLevelEnum.Intermediate.GetEnumKey(),
            TutorNameKey = $"subject.level.{CSubjectLevelEnum.Intermediate.GetEnumKey()}.tutor.name",
            StudentNameKey = $"subject.level.{CSubjectLevelEnum.Intermediate.GetEnumKey()}.student.name",
            DescriptionKey = $"subject.level.{CSubjectLevelEnum.Intermediate.GetEnumKey()}.description",
            IconName = IconConstants.LevelTwo,
            InvertedIconName = IconConstants.LevelTwoInverted
        },
        new CSubjectLevel
        {
            SubjectLevelId = (int)CSubjectLevelEnum.Advanced,
            EnumKey = CSubjectLevelEnum.Advanced.GetEnumKey(),
            TutorNameKey = $"subject.level.{CSubjectLevelEnum.Advanced.GetEnumKey()}.tutor.name",
            StudentNameKey = $"subject.level.{CSubjectLevelEnum.Advanced.GetEnumKey()}.student.name",
            DescriptionKey = $"subject.level.{CSubjectLevelEnum.Advanced.GetEnumKey()}.description",
            IconName = IconConstants.LevelThree,
            InvertedIconName = IconConstants.LevelThreeInverted
        }
    };
}