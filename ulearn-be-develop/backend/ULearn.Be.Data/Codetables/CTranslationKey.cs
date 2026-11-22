using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data.Codetables;

public class CTranslationKey
{
    public CTranslationKey()
    {
        Translations = new HashSet<CTranslation>();
    }

    public int KeyId { get; set; }
    public string KeyName { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }
    public int TranslationNamespaceId { get; set; }

    public virtual CTranslationNamespace TranslationNamespace { get; set; }
    public virtual ICollection<CTranslation> Translations { get; set; }
    public virtual AppUser Creator { get; set; }
    public virtual AppUser Updater { get; set; }
}

public sealed class CTranslationKeyConfiguration : IEntityTypeConfiguration<CTranslationKey>
{
    public void Configure(EntityTypeBuilder<CTranslationKey> entity)
    {
        entity.HasKey(e => e.KeyId)
            .HasName("pk_c_translation_key");

        entity.ToTable("c_translation_key");

        entity.Property(e => e.KeyId).HasColumnName("key_id");
        entity.Property(e => e.KeyName).HasColumnName("key_name");
        entity.Property(e => e.Created).HasColumnName("created");
        entity.Property(e => e.Updated).HasColumnName("updated");
        entity.Property(e => e.CreatorId).HasColumnName("creator_id");
        entity.Property(e => e.UpdaterId).HasColumnName("updater_id");
        entity.Property(e => e.TranslationNamespaceId).HasColumnName("translation_namespace_id");

        entity.HasIndex(e => e.CreatorId, "ix_c_translation_key_creator_id");
        entity.HasIndex(e => e.UpdaterId, "ix_c_translation_key_updater_id");
        entity.HasIndex(e => e.TranslationNamespaceId, "ix_c_translation_key_translation_namespace_id");

        entity.HasIndex(e => e.KeyName, "uc_c_translation_key_key_name").IsUnique();

        entity.HasOne(e => e.Creator)
            .WithMany(e => e.CreatorCTranslationKeys)
            .HasForeignKey(e => e.CreatorId)
            .HasConstraintName("fk_c_translation_key_app_user_creator_id");

        entity.HasOne(e => e.Updater)
            .WithMany(e => e.UpdaterCTranslationKeys)
            .HasForeignKey(e => e.UpdaterId)
            .HasConstraintName("fk_c_translation_key_app_user_updater_id");

        entity.HasOne(e => e.TranslationNamespace)
            .WithMany(e => e.TranslationKeys)
            .HasForeignKey(e => e.TranslationNamespaceId)
            .HasConstraintName("fk_c_translation_key_c_translation_namespace_translation_namespace_id");
    }
}