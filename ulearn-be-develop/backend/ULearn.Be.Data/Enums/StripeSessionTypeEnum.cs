using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum StripeSessionTypeEnum
{
    [EnumKey("reservation_payment")] ReservationPayment = 1,
    [EnumKey("balance_refill")] BalanceRefill = 2,
}