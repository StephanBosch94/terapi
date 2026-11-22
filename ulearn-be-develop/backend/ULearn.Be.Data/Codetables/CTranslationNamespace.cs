using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data.Codetables;

public class CTranslationNamespace
{
    public CTranslationNamespace()
    {
        TranslationKeys = new HashSet<CTranslationKey>();
    }

    public int TranslationNamespaceId { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }

    public virtual ICollection<CTranslationKey> TranslationKeys { get; set; }
    public virtual AppUser Creator { get; set; }
    public virtual AppUser Updater { get; set; }
}

public sealed class CTranslationNamespaceConfiguration : IEntityTypeConfiguration<CTranslationNamespace>
{
    public void Configure(EntityTypeBuilder<CTranslationNamespace> entity)
    {
        entity.HasKey(e => e.TranslationNamespaceId)
            .HasName("pk_c_translation_namespace");

        entity.ToTable("c_translation_namespace");

        entity.Property(e => e.TranslationNamespaceId).HasColumnName("translation_namespace_id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Created).HasColumnName("created");
        entity.Property(e => e.Updated).HasColumnName("updated");
        entity.Property(e => e.CreatorId).HasColumnName("creator_id");
        entity.Property(e => e.UpdaterId).HasColumnName("updater_id");

        entity.HasIndex(e => e.CreatorId, "ix_c_translation_namespace_creator_id");
        entity.HasIndex(e => e.UpdaterId, "ix_c_translation_namespace_updater_id");

        entity.HasOne(e => e.Creator)
            .WithMany(e => e.CreatorCTranslationNamespaces)
            .HasForeignKey(e => e.CreatorId)
            .HasConstraintName("fk_c_translation_namespace_app_user_creator_id");

        entity.HasOne(e => e.Updater)
            .WithMany(e => e.UpdaterCTranslationNamespaces)
            .HasForeignKey(e => e.UpdaterId)
            .HasConstraintName("fk_c_translation_namespace_app_user_updater_id");
    }
}