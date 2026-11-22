using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CNotificationStateEnum
{
    [EnumKey("created")] Created = 1,
    [EnumKey("read")] Read = 2,
    [EnumKey("failed_to_send")] FailedToSend = 3
}