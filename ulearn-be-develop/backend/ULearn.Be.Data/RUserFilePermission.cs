using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class RUserFilePermission
{
    [Column("user_file_permission_id")] public int UserFilePermissionId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("file_path")] public string FilePath { get; set; }
    public virtual AppUser User { get; set; } = null!;
}

public sealed class RUserFileConfiguration : IEntityTypeConfiguration<RUserFilePermission>
{
    public void Configure(EntityTypeBuilder<RUserFilePermission> entity)
    {
        entity.HasKey(e => e.UserFilePermissionId)
            .HasName("pk_r_user_file_permission");

        entity.ToTable("r_user_file_permission");
        
        entity.HasIndex(e => e.UserId, "ix_r_user_file_permission_user_id");
        
        entity.HasIndex(e => new { e.UserId, e.FilePath }, "uc_r_user_file_permission_user_id_file_path")
            .IsUnique();
        
        entity.HasOne(d => d.User)
            .WithMany(p => p.RUserFilePermissions)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_file_permission_app_user_user_id");
    }
}