using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTransactionTypeEnum
{
    [EnumKey("deposit")] Deposit = 1,
    [EnumKey("package_cancellation")] PackageCancellationCreditMemo = 2,
    [EnumKey("withdrawal")] Withdrawal = 3,
    [EnumKey("refund")] RefundCreditMemo = 4,
    [EnumKey("lesson_payment")] LessonPayment = 5,
    [EnumKey("lesson_commission")] LessonCommission = 6,
    [EnumKey("refund_commission")] RefundLessonCommissionCreditMemo = 7,
    [EnumKey("stripe_commission")] StripeCommission = 8,
    [EnumKey("withdraw_commission")] WithdrawCommission = 9, // our transaction

    [EnumKey("package_sale_cancellation_commission")]
    PackageSaleCancellationCommission = 10,

    [EnumKey("money_transfer")] MoneyTransfer = 11
}