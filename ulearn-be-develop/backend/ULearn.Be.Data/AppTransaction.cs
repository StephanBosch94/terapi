using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class AppTransaction
    {
        [Column("transaction_id")] public int TransactionId { get; set; }
        [Column("user_wallet_id")] public int UserWalletId { get; set; }
        [Column("transaction_group_id")] public int TransactionGroupId { get; set; }
        [Column("transaction_type_id")] public int TransactionTypeId { get; set; }
        [Column("amount")] public double Amount { get; set; }
        [Column("created")] public DateTime Created { get; set; }
        [Column("updated")] public DateTime Updated { get; set; }
        [Column("approved")] public DateTime? Approved { get; set; }
        [Column("transaction_state_id")] public int TransactionStateId { get; set; }
        [Column("planned_completion_time")] public DateTime PlannedCompletionTime { get; set; }

        [Column("payment_service_provider_id")]
        public int? PaymentServiceProviderId { get; set; }

        [Column("reservation_package_id")] public int? ReservationPackageId { get; set; }
        [Column("reservation_id")] public int? ReservationId { get; set; }
        [Column("reservation_issue_id")] public int? ReservationIssueId { get; set; }
        [Column("session_id")] public string SessionId { get; set; }
        [Column("creator_id")] public int CreatorId { get; set; }
        [Column("updater_id")] public int? UpdaterId { get; set; }
        [Column("approver_id")] public int? ApproverId { get; set; }
        [Column("bank_account_number")] public string BankAccountNumber { get; set; }
        [Column("bank_code")] public string BankCode { get; set; }
        [Column("currency_id")] public int CurrencyId { get; set; }
        [Column("source_transaction_id")] public int? SourceTransactionId { get; set; }
        [Column("receipt_url")] public string ReceiptUrl { get; set; }
        [Column("vendor_id")] public int VendorId { get; set; }
        [Column("customer_id")] public int CustomerId { get; set; }

        [Column("transaction_refund_reason_id")]
        public int? TransactionRefundReasonId { get; set; }

        [Column("coupon_id")] public int? CouponId { get; set; }

        [Column("coupon_sale_amount")] public double? CouponSaleAmount { get; set; }
        public virtual CPaymentServiceProvider PaymentServiceProvider { get; set; } = null!;
        public virtual CTransactionState TransactionState { get; set; } = null!;
        public virtual CTransactionType TransactionType { get; set; } = null!;
        public virtual AppUserWallet UserWallet { get; set; } = null!;
        public virtual AppUser Updater { get; set; } = null!;
        public virtual AppUser Creator { get; set; } = null!;
        public virtual AppUser Approver { get; set; } = null!;
        public virtual AppUser Vendor { get; set; } = null!;
        public virtual AppUser Customer { get; set; } = null!;
        public virtual AppReservationPackage AppReservationPackage { get; set; }
        public virtual AppReservation AppReservation { get; set; }
        public virtual AppReservationIssue AppReservationIssue { get; set; }
        public virtual CCurrency Currency { get; set; }
        public virtual CTransactionRefundReason CTransactionRefundReason { get; set; }
        public virtual AppTransaction SourceAppTransaction { get; set; }
        public virtual AppTransactionGroup AppTransactionGroup { get; set; }
        public virtual AppCoupon AppCoupon { get; set; }
        public virtual ICollection<AppTransaction> TargetAppTransactions { get; set; }
    }
}

public sealed class AppUserWalletTransactionConfiguration : IEntityTypeConfiguration<AppTransaction>
{
    public void Configure(EntityTypeBuilder<AppTransaction> entity)
    {
        entity.HasKey(e => e.TransactionId)
            .HasName("pk_app_transaction");

        entity.ToTable("app_transaction");

        entity.HasIndex(
            e => e.PaymentServiceProviderId,
            "ix_app_transaction_payment_service_provider_id"
        );

        entity.HasIndex(e => e.TransactionStateId, "ix_app_transaction_transaction_state_id");
        entity.HasIndex(e => e.TransactionTypeId, "ix_app_transaction_transaction_type_id");
        entity.HasIndex(e => e.UserWalletId, "ix_app_transaction_user_wallet_id");
        entity.HasIndex(e => e.TransactionGroupId, "ix_app_transaction_transaction_group_id");
        entity.HasIndex(e => e.ReservationPackageId, "ix_app_transaction_reservation_package_id");
        entity.HasIndex(e => e.ReservationId, "ix_app_transaction_reservation_id");
        entity.HasIndex(e => e.ReservationIssueId, "ix_app_transaction_reservation_reservation_issue_id");
        entity.HasIndex(e => e.CreatorId, "ix_app_transaction_creator_id");
        entity.HasIndex(e => e.UpdaterId, "ix_app_transaction_updater_id");
        entity.HasIndex(e => e.ApproverId, "ix_app_transaction_approver_id");
        entity.HasIndex(e => e.CurrencyId, "ix_app_transaction_currency_id");
        entity.HasIndex(e => e.SourceTransactionId, "ix_app_transaction_source_transaction_id");
        entity.HasIndex(e => e.VendorId, "ix_app_transaction_vendor_id");
        entity.HasIndex(e => e.CustomerId, "ix_app_transaction_customer_id");
        entity.HasIndex(e => e.TransactionRefundReasonId, "ix_app_transaction_refund_reason_id");
        entity.HasIndex(e => e.CouponId, "ix_app_transaction_coupon_id");

        entity.HasOne(d => d.PaymentServiceProvider)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.PaymentServiceProviderId)
            .HasConstraintName("fk_app_transaction_c_payment_service_provider_payment_service_provider_id");

        entity.HasOne(d => d.TransactionState)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.TransactionStateId)
            .HasConstraintName("fk_app_transaction_c_transaction_state_transaction_state_id");

        entity.HasOne(d => d.TransactionType)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.TransactionTypeId)
            .HasConstraintName("fk_app_transaction_c_transaction_type_transaction_type_id");

        entity.HasOne(d => d.UserWallet)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.UserWalletId)
            .HasConstraintName("fk_app_transaction_app_user_wallet_user_wallet_id");

        entity.HasOne(d => d.AppTransactionGroup)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.TransactionGroupId)
            .HasConstraintName("fk_app_transaction_app_transaction_group_transaction_group_id");

        entity.HasOne(d => d.AppReservationPackage)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.ReservationPackageId)
            .HasConstraintName("fk_app_transaction_app_reservation_package_reservation_package_id");

        entity.HasOne(d => d.Updater)
            .WithMany(p => p.UpdatedTransactions)
            .HasForeignKey(d => d.UpdaterId)
            .HasConstraintName("fk_app_transaction_app_user_updater_id");

        entity.HasOne(d => d.Creator)
            .WithMany(p => p.CreatedTransactions)
            .HasForeignKey(d => d.CreatorId)
            .HasConstraintName("fk_app_transaction_app_user_creator_id");

        entity.HasOne(d => d.Approver)
            .WithMany(p => p.ApprovedTransactions)
            .HasForeignKey(d => d.ApproverId)
            .HasConstraintName("fk_app_transaction_app_user_approver_id");

        entity.HasOne(d => d.AppReservationIssue)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.ReservationIssueId)
            .HasConstraintName("fk_app_transaction_app_reservation_issue_reservation_issue_id");

        entity.HasOne(d => d.AppReservation)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.ReservationId)
            .HasConstraintName("fk_app_transaction_app_reservation_reservation_id");

        entity.HasOne(d => d.Currency)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.CurrencyId)
            .HasConstraintName("fk_app_transaction_app_currency_currency_id");

        entity.HasOne(d => d.SourceAppTransaction)
            .WithMany(p => p.TargetAppTransactions)
            .HasForeignKey(d => d.SourceTransactionId)
            .HasConstraintName("fk_app_transaction_app_transaction_source_transaction_id");

        entity.HasOne(d => d.Vendor)
            .WithMany(p => p.VendorTransactions)
            .HasForeignKey(d => d.VendorId)
            .HasConstraintName("fk_app_transaction_app_user_vendor_id");

        entity.HasOne(d => d.Customer)
            .WithMany(p => p.CustomerTransactions)
            .HasForeignKey(d => d.CustomerId)
            .HasConstraintName("fk_app_transaction_app_user_customer_id");

        entity.HasOne(d => d.CTransactionRefundReason)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.TransactionRefundReasonId)
            .HasConstraintName("fk_app_transaction_c_refund_reason_refund_reason_id");

        entity.HasOne(d => d.AppCoupon)
            .WithMany(p => p.AppTransactions)
            .HasForeignKey(d => d.CouponId)
            .HasConstraintName("fk_app_transaction_app_coupon_coupon_id");
    }
}