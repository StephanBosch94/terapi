using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CTransactionTemplateType : CodetableBase
{
    [Column("transaction_template_type_id")]
    public int TransactionTemplateTypeId { get; set; }
}

public sealed class CTransactionTemplateTypeConfiguration : IEntityTypeConfiguration<CTransactionTemplateType>
{
    public void Configure(EntityTypeBuilder<CTransactionTemplateType> entity)
    {
        entity.HasKey(e => e.TransactionTemplateTypeId)
            .HasName("pk_c_transaction_template_type");

        entity.ToTable("c_transaction_template_type");

        entity.HasData(CTransactionTemplateTypeSeeder.Entities);
    }
}