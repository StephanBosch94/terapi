using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CCategoryEnum
{
    [EnumKey("languages")] Languages = 1,
    [EnumKey("music")] Music = 2,
    [EnumKey("school")] School = 3,
    [EnumKey("programming")] Programming = 4
}