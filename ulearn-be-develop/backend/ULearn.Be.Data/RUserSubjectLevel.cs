using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class RUserSubjectLevel
{
    [Column("user_subject_level_id")] public int UserSubjectLevelId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("subject_level_id")] public int SubjectLevelId { get; set; }
    public AppUser AppUser { get; set; }
    public CSubjectLevel CSubjectLevel { get; set; }
}

public sealed class RUserSubjectLevelConfiguration : IEntityTypeConfiguration<RUserSubjectLevel>
{
    public void Configure(EntityTypeBuilder<RUserSubjectLevel> entity)
    {
        entity.HasKey(e => e.UserSubjectLevelId)
            .HasName("pk_r_user_subject_level");

        entity.ToTable("r_user_subject_level");

        entity.HasIndex(e => e.UserId, "ix_r_user_subject_level_user_id");
        entity.HasIndex(e => e.SubjectLevelId, "ix_r_user_subject_level_subject_level_id");
        entity.HasIndex(e => new { e.UserId, e.SubjectLevelId }, "uc_r_user_subject_level_user_id_subject_level_id")
            .IsUnique();
        
        entity.HasOne(e => e.AppUser)
            .WithMany(e => e.RUserSubjectLevels)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_r_user_subject_level_app_user_user_id");

        entity.HasOne(e => e.CSubjectLevel)
            .WithMany(e => e.RUserSubjectLevels)
            .HasForeignKey(e => e.SubjectLevelId)
            .HasConstraintName("fk_r_user_subject_level_c_subject_level_subject_level_id");
    }
}