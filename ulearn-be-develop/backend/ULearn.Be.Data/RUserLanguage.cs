using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class RUserLanguage
    {
        public int UserLanguageId { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public int LanguageLevelId { get; set; }
        public bool IsDefault { get; set; }

        public virtual CLanguage Language { get; set; } = null!;
        public virtual CLanguageLevel LanguageLevel { get; set; } = null!;
        public virtual AppUser User { get; set; } = null!;
    }
}

public sealed class RUserLanguageConfiguration : IEntityTypeConfiguration<RUserLanguage>
{
    public void Configure(EntityTypeBuilder<RUserLanguage> entity)
    {
        entity.HasKey(e => e.UserLanguageId)
            .HasName("pk_r_user_language");

        entity.ToTable("r_user_language");

        entity.HasIndex(e => e.LanguageId, "ix_r_user_language_language_id");

        entity.HasIndex(e => e.LanguageLevelId, "ix_r_user_language_language_level_id");

        entity.HasIndex(e => e.UserId, "ix_r_user_language_user_id");

        entity.Property(e => e.UserLanguageId).HasColumnName("user_language_id");

        entity.Property(e => e.IsDefault).HasColumnName("is_default");

        entity.Property(e => e.LanguageId).HasColumnName("language_id");

        entity.Property(e => e.LanguageLevelId).HasColumnName("language_level_id");

        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasIndex(e => new { e.UserId, e.LanguageId }, "uc_r_user_language_user_id_language_id")
            .IsUnique();
        
        entity.HasOne(d => d.Language)
            .WithMany(p => p.RUserLanguages)
            .HasForeignKey(d => d.LanguageId)
            .HasConstraintName("fk_r_user_language_c_language_language_id");

        entity.HasOne(d => d.LanguageLevel)
            .WithMany(p => p.RUserLanguages)
            .HasForeignKey(d => d.LanguageLevelId)
            .HasConstraintName("fk_r_user_language_c_language_level_language_level_id");

        entity.HasOne(d => d.User)
            .WithMany(p => p.RUserLanguages)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_language_app_user_user_id");
    }
}