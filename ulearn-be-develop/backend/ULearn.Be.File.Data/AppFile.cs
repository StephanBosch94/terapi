using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Constants;
using ULearn.Be.File.Data.Enums;

namespace ULearn.Be.File.Data;

public class AppFile
{
    [Column("file_id")] public int FileId { get; set; }
    [Column("unique_key")] public string UniqueKey { get; set; }
    [Column("content_type")] public string ContentType { get; set; }
    [Column("path")] public string Path { get; set; }
    [Column("file_name")] public string FileName { get; set; }
    [Column("environment")] public string Environment { get; set; }
    [Column("creator_id")] public int CreatorId { get; set; }
    [Column("updater_id")] public int UpdaterId { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("updated")] public DateTime Updated { get; set; }
    [Column("state_id")] public int StateId { get; set; } = (int)CFileStateEnum.Active;
    [Column("compression_type_id")] public int CompressionTypeId { get; set; }
    public virtual CFileState CFileState { get; set; }
    public virtual CCompressionType CCompressionType { get; set; }
}

public sealed class AppFileConfiguration : IEntityTypeConfiguration<AppFile>
{
    public void Configure(EntityTypeBuilder<AppFile> entity)
    {
        entity.HasKey(e => e.FileId).HasName("pk_app_file");

        entity.ToTable("app_file", DatabaseSchemaConstants.Sfile);

        entity.HasIndex(e => e.StateId, "ix_app_file_c_state_state_id");
        entity.HasIndex(e => e.CompressionTypeId, "ix_app_file_c_compression_type_compression_type_id");
        
        entity.HasOne(e => e.CFileState)
            .WithMany(e => e.AppFiles)
            .HasForeignKey(e => e.StateId)
            .HasConstraintName("fk_app_file_c_file_state_file_state_id");

        entity.HasOne(e => e.CCompressionType)
            .WithMany(e => e.AppFiles)
            .HasForeignKey(e => e.CompressionTypeId)
            .HasConstraintName("fk_app_file_c_compression_type_compression_type_id");
    }
}