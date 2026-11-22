using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CLanguageLevelEnum
{
    [EnumKey("a1")] A1 = 1,
    [EnumKey("a2")] A2 = 2,
    [EnumKey("b1")] B1 = 3,
    [EnumKey("b2")] B2 = 4,
    [EnumKey("c1")] C1 = 5,
    [EnumKey("c2")] C2 = 6,
    [EnumKey("native")] Native = 7
}