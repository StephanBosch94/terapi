using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CRoomStateEnum
{
    [EnumKey("created")] Created = 1,
    [EnumKey("started")] Started = 2
}