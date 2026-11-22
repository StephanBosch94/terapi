using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data.Codetables;

public class CTranslation
{
    [Column("Translation_Id")] public int TranslationId { get; set; }
    [Column("Key_Id")] public int TranslationKeyId { get; set; }
    [Column("Language_Id")] public int TranslationLanguageId { get; set; }
    [Column("Text_Value")] public string TextValue { get; set; }
    [Column("Is_Protected")] public bool IsProtected { get; set; }
    [Column("Created")] public DateTime Created { get; set; }
    [Column("Updated")] public DateTime? Updated { get; set; }
    [Column("Creator_Id")] public int CreatorId { get; set; }
    [Column("Updater_Id")] public int? UpdaterId { get; set; }

    public virtual AppUser Creator { get; set; }
    public virtual AppUser Updater { get; set; }
    public virtual CTranslationKey TranslationKey { get; set; }
    public virtual CTranslationLanguage CTranslationLanguage { get; set; }
}

public sealed class CTranslationConfiguration : IEntityTypeConfiguration<CTranslation>
{
    public void Configure(EntityTypeBuilder<CTranslation> entity)
    {
        entity.HasKey(e => e.TranslationId)
            .HasName("pk_c_translation");

        entity.ToTable("c_translation");

        entity.Property(e => e.TranslationId).HasColumnName("translation_id");
        entity.Property(e => e.TranslationKeyId).HasColumnName("translation_key_id");
        entity.Property(e => e.TranslationLanguageId).HasColumnName("translation_language_id");
        entity.Property(e => e.TextValue).HasColumnName("text_value");
        entity.Property(e => e.IsProtected).HasColumnName("is_protected");
        entity.Property(e => e.Created).HasColumnName("created");
        entity.Property(e => e.Updated).HasColumnName("updated");
        entity.Property(e => e.CreatorId).HasColumnName("creator_id");
        entity.Property(e => e.UpdaterId).HasColumnName("updater_id");

        entity.HasIndex(e => e.CreatorId, "ix_c_translation_creator_id");
        entity.HasIndex(e => e.UpdaterId, "ix_c_translation_updater_id");
        entity.HasIndex(e => e.TranslationKeyId, "ix_c_translation_translation_key_id");

        entity.HasOne(e => e.Creator)
            .WithMany(e => e.CreatorCTranslations)
            .HasForeignKey(e => e.CreatorId)
            .HasConstraintName("fk_c_translation_app_user_creator_id");

        entity.HasOne(e => e.Updater)
            .WithMany(e => e.UpdaterCTranslations)
            .HasForeignKey(e => e.UpdaterId)
            .HasConstraintName("fk_c_translation_app_user_updater_id");

        entity.HasOne(e => e.TranslationKey)
            .WithMany(e => e.Translations)
            .HasForeignKey(e => e.TranslationKeyId)
            .HasConstraintName("fk_c_translation_c_translation_key_translation_id");

        entity.HasOne(e => e.CTranslationLanguage)
            .WithMany(e => e.CTranslations)
            .HasForeignKey(e => e.TranslationLanguageId)
            .HasConstraintName("fk_c_translation_c_translation_language_translation_language_id");
    }
}