using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data.Codetables;

public class CTranslationLanguage
{
    public CTranslationLanguage()
    {
        CTranslations = new HashSet<CTranslation>();
    }

    public int TranslationLanguageId { get; set; }
    public string LanguageCode { get; set; }
    public string Language { get; set; }

    public virtual ICollection<CTranslation> CTranslations { get; set; }
    public virtual ICollection<AppUser> AppUsers { get; set; }
}

public sealed class CTranslationLanguageConfiguration : IEntityTypeConfiguration<CTranslationLanguage>
{
    public void Configure(EntityTypeBuilder<CTranslationLanguage> entity)
    {
        entity.HasKey(e => e.TranslationLanguageId)
            .HasName("pk_c_translation_language");

        entity.ToTable("c_translation_language");

        entity.HasIndex(e => e.LanguageCode, "ix_c_translation_language_language_code");
        entity.Property(e => e.TranslationLanguageId).HasColumnName("translation_language_id");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.Language).HasColumnName("language");
    }
}