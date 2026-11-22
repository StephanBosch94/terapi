using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppTransactionGroup
{
    [Column("transaction_group_id")] public int TransactionGroupId { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    public ICollection<AppTransaction> AppTransactions { get; set; }
}

public sealed class AppTransactionGroupConfiguration : IEntityTypeConfiguration<AppTransactionGroup>
{
    public void Configure(EntityTypeBuilder<AppTransactionGroup> entity)
    {
        entity.HasKey(e => e.TransactionGroupId)
            .HasName("pk_app_transaction_group");

        entity.ToTable("app_transaction_group");
    }
}