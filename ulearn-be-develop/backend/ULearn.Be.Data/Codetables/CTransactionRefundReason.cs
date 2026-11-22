using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CTransactionRefundReason: CodetableBase
{
    [EnumKey("transaction_refund_reason_id")] public int TransactionRefundReasonId { get; set; }
    public ICollection<AppTransaction> AppTransactions { get; set; }
}

public sealed class CTransactionRefundReasonConfiguration : IEntityTypeConfiguration<CTransactionRefundReason>
{
    public void Configure(EntityTypeBuilder<CTransactionRefundReason> entity)
    {
        entity.HasKey(e => e.TransactionRefundReasonId)
            .HasName("pk_c_transaction_refund_reason");

        entity.ToTable("c_transaction_refund_reason");
        
        entity.HasIndex(e => e.EnumKey, "ix_c_transaction_refund_reason").IsUnique();

        entity.HasData(CTransactionRefundReasonSeeder.Entities);
    }
}