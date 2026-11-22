using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTransactionStateEnum
{
    [EnumKey("scheduled")] Scheduled = 1,
    [EnumKey("completed")] Completed = 2,
    [EnumKey("canceled")] Canceled = 3,
    [EnumKey("waiting_for_review")] WaitingForReview = 4,
    [EnumKey("pending")] Pending = 5,
    [EnumKey("created")] Created = 6
}