using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTeachingExperienceEnum
{
    [EnumKey("formal_teaching_experience")]
    FormalTeachingExperience = 1,

    [EnumKey("informal_teaching_experience")]
    InformalTeachingExperience = 2,
    [EnumKey("none")] None = 3
}