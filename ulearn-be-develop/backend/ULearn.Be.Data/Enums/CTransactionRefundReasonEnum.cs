using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTransactionRefundReasonEnum
{
    [EnumKey("package_cancellation")] PackageCancellation = 1,
    [EnumKey("tutor_reschedule")] TutorRescheduled = 2,
    [EnumKey("student_reschedule")] StudentRescheduled = 3,
    [EnumKey("refund")] Refund = 4
}