using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CReservationStateEnum
{
    [EnumKey("created")] Created = 1,
    [EnumKey("canceled_by_user")] CanceledByUser = 2,
    [EnumKey("failed_payment")] FailedPayment = 3,
    [EnumKey("confirmed")] Confirmed = 4,
    [EnumKey("scheduled_for_payment")] ScheduledForPayment = 5,
    [EnumKey("payed")] Payed = 6,
    [EnumKey("refunded")] Refunded = 7,
    [EnumKey("waiting_for_review")] WaitingForReview = 8,
    [EnumKey("rescheduled")] Rescheduled = 9
}