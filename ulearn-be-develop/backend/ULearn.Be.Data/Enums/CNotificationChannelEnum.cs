using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CNotificationChannelEnum
{
    [EnumKey("application")] Application = 1,
    [EnumKey("email")] Email = 2,
    [EnumKey("slack")] Slack = 3
}