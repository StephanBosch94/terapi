using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Constants;

namespace ULearn.Be.File.Data;

public class RFileFormatCompressionType
{
    [Column("file_format_id")] public int FileFormatId { get; set; }
    [Column("compression_type_id2")] public int CompressionTypeId { get; set; }
    public virtual CCompressionType CompressionType { get; set; }
    public virtual CFileFormat FileFormat { get; set; }
}

public sealed class RFileFormatCompressionTypeConfiguration : IEntityTypeConfiguration<RFileFormatCompressionType>
{
    public void Configure(EntityTypeBuilder<RFileFormatCompressionType> entity)
    {
        entity.HasKey(e => new { e.FileFormatId, e.CompressionTypeId }).HasName("pk_r_file_compression_type");

        entity.ToTable("r_file_format_compression_type", DatabaseSchemaConstants.Sfile);

        entity.HasIndex(e => e.FileFormatId, "ix_r_file_format_compression_type_c_file_format_file_format_id");
        entity.HasIndex(e => e.CompressionTypeId, "ix_r_file_format_compression_type_c_compression_type_compression_type_id");
                
        entity.HasOne(e => e.CompressionType)
            .WithMany(e => e.RFileFormatCompressionTypes)
            .HasForeignKey(e => e.CompressionTypeId)
            .HasConstraintName("fk_r_file_format_compression_type_c_compression_type_compression_type_id");

        entity.HasOne(e => e.FileFormat)
            .WithMany(e => e.RFileFormatCompressionTypes)
            .HasForeignKey(e => e.FileFormatId)
            .HasConstraintName("fk_r_file_format_compression_type_c_file_format_file_format_id");
    }
}