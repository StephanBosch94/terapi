using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTransactionTemplateTypeEnum
{
    [EnumKey("tutors_receipt_with_taxes")] TutorsReceiptWithTaxes = 1,

    [EnumKey("tutors_receipt_without_taxes")]
    TutorsReceiptWithoutTaxes = 2,

    [EnumKey("tutors_credit_memo")] TutorsCreditMemo = 3,

    [EnumKey("platforms_commission_receipt")]
    PlatformsCommissionReceipt = 4,
        
    [EnumKey("platforms_withdrawal_confirmation")]
    PlatformsWithdrawalConfirmation = 5,

    [EnumKey("platforms_credit_memo")] PlatformsCreditMemo = 6,
    
    [EnumKey("money_transfer_confirmation")] MoneyTransferConfirmation = 7
}