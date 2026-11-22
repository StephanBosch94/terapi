using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CMessageStateEnum
{
    [EnumKey("created")] Created = 1,
    [EnumKey("read")] Read = 2,
}