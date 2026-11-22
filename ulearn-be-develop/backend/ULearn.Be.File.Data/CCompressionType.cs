using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Constants;
using ULearn.Be.File.Data.Seeders;

namespace ULearn.Be.File.Data;

public class CCompressionType 
{
    [Column("compression_type_id")] public int CompressionTypeId { get; set; }
    [Column("enum_Key")] public string EnumKey { get; set; }
    [Column("name_Key")] public string NameKey { get; set; }
    [Column("description_Key")] public string DescriptionKey { get; set; }
    public virtual ICollection<RFileFormatCompressionType> RFileFormatCompressionTypes { get; set; }
    public virtual ICollection<AppFile> AppFiles { get; set; }
}

public sealed class CCompressionTypeConfiguration : IEntityTypeConfiguration<CCompressionType>
{
    public void Configure(EntityTypeBuilder<CCompressionType> entity)
    {
        entity.HasKey(e => e.CompressionTypeId).HasName("pk_c_compression_type");

        entity.ToTable("c_compression_type", DatabaseSchemaConstants.Sfile);
        
        entity.HasData(CCompressionTypeSeeder.Entities);
    }
}