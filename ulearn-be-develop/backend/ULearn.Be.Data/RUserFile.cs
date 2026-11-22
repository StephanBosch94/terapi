using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class RUserFile
    {
       [Column("user_file_id")] public int UserFileId { get; set; }
       [Column("user_id")] public int UserId { get; set; }
       [Column("file_type_id")] public int FileTypeId { get; set; }
       [Column("file_name")] public string FileName { get; set; }
       [Column("content_type")] public string ContentType { get; set; }
       [Column("file_path")] public string FilePath { get; set; }
       [Column("content")] public byte[] Content { get; set; }
       public virtual CFileType FileType { get; set; } = null!;
       public virtual AppUser User { get; set; } = null!;
    }
}

public sealed class RUserFileConfiguration : IEntityTypeConfiguration<RUserFile>
{
    public void Configure(EntityTypeBuilder<RUserFile> entity)
    {
        entity.HasKey(e => e.UserFileId)
            .HasName("pk_r_user_file");

        entity.ToTable("r_user_file");

        entity.HasIndex(e => e.FileTypeId, "ix_r_user_file_file_type_id");

        entity.HasIndex(e => e.UserId, "ix_r_user_file_user_id");
        
        entity.HasOne(d => d.FileType)
            .WithMany(p => p.RUserFiles)
            .HasForeignKey(d => d.FileTypeId)
            .HasConstraintName("fk_r_user_file_c_file_type_file_type_id");

        entity.HasOne(d => d.User)
            .WithMany(p => p.RUserFiles)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_file_app_user_user_id");
    }
}