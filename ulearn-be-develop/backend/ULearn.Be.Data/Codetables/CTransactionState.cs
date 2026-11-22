using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CTransactionState : CodetableBase
    {
        public CTransactionState()
        {
            AppTransactions = new HashSet<AppTransaction>();
        }

        public int TransactionStateId { get; set; }
        public virtual ICollection<AppTransaction> AppTransactions { get; set; }
    }
}

public sealed class CTransactionStateConfiguration : IEntityTypeConfiguration<CTransactionState>
{
    public void Configure(EntityTypeBuilder<CTransactionState> entity)
    {
        entity.HasKey(e => e.TransactionStateId)
            .HasName("pk_c_transaction_state");

        entity.ToTable("c_transaction_state");

        entity.Property(e => e.TransactionStateId).HasColumnName("transaction_state_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_transaction_state_enum_key").IsUnique();

        entity.HasData(CTransactionStateSeeder.Entities);
    }
}