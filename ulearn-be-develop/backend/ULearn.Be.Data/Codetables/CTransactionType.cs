using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CTransactionType : CodetableBase
    {
        public CTransactionType()
        {
            AppTransactions = new HashSet<AppTransaction>();
        }

        public int TransactionTypeId { get; set; }
        public virtual ICollection<AppTransaction> AppTransactions { get; set; }
    }
}

public sealed class CTransactionTypeConfiguration : IEntityTypeConfiguration<CTransactionType>
{
    public void Configure(EntityTypeBuilder<CTransactionType> entity)
    {
        entity.HasKey(e => e.TransactionTypeId)
            .HasName("pk_c_transaction_type");

        entity.ToTable("c_transaction_type");

        entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_transaction_type_enum_key").IsUnique();

        entity.HasData(CTransactionTypeSeeder.Entities);
    }
}