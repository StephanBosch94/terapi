using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CObjectStateEnum
{
    [EnumKey("active")] Active = 1,
    [EnumKey("deleted")] Deleted = 2,
}