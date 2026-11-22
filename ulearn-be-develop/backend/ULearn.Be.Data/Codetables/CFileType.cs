using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CFileType : CodetableBase
    {
        public CFileType()
        {
            RUserFiles = new HashSet<RUserFile>();
        }

        public int FileTypeId { get; set; }
        public virtual ICollection<RUserFile> RUserFiles { get; set; }
    }
}

public sealed class CFileTypeConfiguration : IEntityTypeConfiguration<CFileType>
{
    public void Configure(EntityTypeBuilder<CFileType> entity)
    {
        entity.HasKey(e => e.FileTypeId)
            .HasName("pk_c_file_type");

        entity.ToTable("c_file_type");

        entity.Property(e => e.FileTypeId).HasColumnName("file_type_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_file_type_enum_key").IsUnique();

        entity.HasData(CFileTypeSeeder.Entities);
    }
}