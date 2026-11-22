using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CSpecializationCategoryEnum
{
    [EnumKey("popular")] Popular = 1,

    [EnumKey("other")] Other = 2,

    [EnumKey("accent")] Accent = 3,

    [EnumKey("tests")] Tests = 4,
}