using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CSubjectLevelEnum
{
    [EnumKey("beginner")] Beginner = 1,
    [EnumKey("intermediate")] Intermediate = 2,
    [EnumKey("advanced")] Advanced = 3
}