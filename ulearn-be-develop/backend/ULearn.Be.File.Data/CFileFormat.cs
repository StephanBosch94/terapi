using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Constants;

namespace ULearn.Be.File.Data;

public class CFileFormat
{
    [Column("file_format_id")] public int FileFormatId { get; set; }
    [Column("extension")] public string Extension { get; set; }
    [Column("enum_key")] public string EnumKey { get; set; }
    [Column("name_key")] public string NameKey { get; set; }
    [Column("description_key")] public string DescriptionKey { get; set; }
    public virtual ICollection<RFileFormatCompressionType> RFileFormatCompressionTypes { get; set; }
}

public sealed class CFileFormatConfiguration : IEntityTypeConfiguration<CFileFormat>
{
    public void Configure(EntityTypeBuilder<CFileFormat> entity)
    {
        entity.HasKey(e => e.FileFormatId).HasName("pk_c_file_format");

        entity.ToTable("c_file_format", DatabaseSchemaConstants.Sfile);
    }
}