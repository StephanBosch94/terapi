using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CRoomActivityTypeEnum
{
    [EnumKey("disconnected")] Disconnected = 1,
    [EnumKey("joined")] Join = 2,
    [EnumKey("started")] Start = 3,
    [EnumKey("signal")] Signal = 4,
    [EnumKey("connected")] Connected = 5,
}