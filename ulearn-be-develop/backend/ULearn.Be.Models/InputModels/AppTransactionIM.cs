using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class AppTransactionIM
{
    public AppTransactionIM(
        int creatorId,
        int vendorId,
        int customerId,
        int ownerWalletId,
        CTransactionTypeEnum transactionTypeEnum,
        CTransactionStateEnum transactionStateEnum,
        double amount,
        DateTime plannedCompletionTime,
        string sessionId = "",
        int? reservationPackageId = null,
        int? reservationId = null,
        int? reservationIssueId = null,
        int? sourceTransactionId = null,
        string bankAccountNumber = "",
        string bankCode = "",
        string receiptUrl = "",
        CTransactionRefundReasonEnum? transactionRefundReasonEnum = null,
        int? couponId = null,
        double? couponSaleAmount = null
    )
    {
        OwnerWalletId = ownerWalletId;
        CustomerId = customerId;
        VendorId = vendorId;
        TransactionTypeEnum = transactionTypeEnum;
        TransactionStateEnum = transactionStateEnum;
        Amount = amount;
        PlannedCompletionTime = plannedCompletionTime;
        SessionId = sessionId;
        ReservationPackageId = reservationPackageId;
        ReservationId = reservationId;
        ReservationIssueId = reservationIssueId;
        SourceTransactionId = sourceTransactionId;
        BankAccountNumber = bankAccountNumber;
        BankCode = bankCode;
        CreatorId = creatorId;
        ReceiptUrl = receiptUrl;
        TransactionRefundReasonEnum = transactionRefundReasonEnum;
        CouponId = couponId;
        CouponSaleAmount = couponSaleAmount;
    }

    public int OwnerWalletId { get; set; }
    public int CustomerId { get; set; }
    public int VendorId { get; set; }
    public CTransactionTypeEnum TransactionTypeEnum { get; set; }
    public CTransactionStateEnum TransactionStateEnum { get; set; }
    public CTransactionRefundReasonEnum? TransactionRefundReasonEnum { get; set; }
    public double Amount { get; set; }
    public string SessionId { get; set; }
    public int? ReservationPackageId { get; set; }
    public int? ReservationId { get; set; }
    public int? ReservationIssueId { get; set; }
    public int? SourceTransactionId { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankCode { get; set; }
    public int CreatorId { get; set; }
    public int? CouponId { get; set; }
    public double? CouponSaleAmount { get; set; }
    public string ReceiptUrl { get; set; }
    public DateTime PlannedCompletionTime { get; set; }
}