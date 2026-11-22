using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CReservationIssueStateEnum
{
    [EnumKey("waiting")] [IconName("waiting")]
    Waiting = 1,

    [EnumKey("approved")] [IconName("approved")]
    Approved = 2,

    [EnumKey("declined")] [IconName("declined")]
    Declined = 3
}