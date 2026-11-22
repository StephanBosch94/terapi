using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data.Codetables;

public class CTransactionTemplate
{
    [Column("transaction_template_id")] public int TransactionTemplateId { get; set; }

    [Column("transaction_type_template_id")]
    public int TransactionTemplateTypeId { get; set; }

    [Column("content_type")] public string ContentType { get; set; }
    [Column("data")] public byte[] Data { get; set; }
    public virtual CTransactionTemplateType CTransactionTemplateType { get; set; }
}

public sealed class CTransactionTemplateConfiguration : IEntityTypeConfiguration<CTransactionTemplate>
{
    public void Configure(EntityTypeBuilder<CTransactionTemplate> entity)
    {
        entity.HasKey(e => e.TransactionTemplateId)
            .HasName("pk_c_transaction_template");

        entity.ToTable("c_transaction_template");

        entity.HasIndex(e => e.TransactionTemplateTypeId, "uc_c_transaction_template_transaction_template_type_id")
            .IsUnique();

        entity.HasOne(e => e.CTransactionTemplateType)
            .WithMany()
            .HasForeignKey(e => e.TransactionTemplateTypeId);
    }
}