using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CUserAttributeTypeEnum
{
    [EnumKey("description")] Description = 1,
    [EnumKey("video_url")] VideoUrl = 2
}