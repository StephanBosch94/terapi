using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CMasterHash : CodetableBase
{
    public int MasterHashId { get; set; }
    public string TableName { get; set; }
    public Guid Hash { get; set; }
    public DateTime Updated { get; set; }
}

public sealed class CMasterHashConfiguration : IEntityTypeConfiguration<CMasterHash>
{
    public void Configure(EntityTypeBuilder<CMasterHash> entity)
    {
        entity.HasKey(e => e.MasterHashId)
            .HasName("pk_c_master_hash");

        entity.ToTable("c_master_hash");

        entity.Property(e => e.MasterHashId).HasColumnName("master_hash_id");

        entity.Property(e => e.TableName).HasColumnName("table_name");

        entity.Property(e => e.Hash).HasColumnName("hash");
        
        entity.Property(e => e.Updated).HasColumnName("updated");

        entity.HasIndex(e => e.TableName, "ix_c_master_hash_table_name");

        entity.HasIndex(e => e.EnumKey, "ix_c_master_hash_enum_key").IsUnique();

        entity.HasData(CMasterHashSeeder.Entities);
    }
}