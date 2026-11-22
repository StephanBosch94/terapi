using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CLanguageLevel : CodetableBase
    {
        public CLanguageLevel()
        {
            RUserLanguages = new HashSet<RUserLanguage>();
        }

        public int LanguageLevelId { get; set; }
        public virtual ICollection<RUserLanguage> RUserLanguages { get; set; }
    }
}

public sealed class CLanguageLevelConfiguration : IEntityTypeConfiguration<CLanguageLevel>
{
    public void Configure(EntityTypeBuilder<CLanguageLevel> entity)
    {
        entity.HasKey(e => e.LanguageLevelId)
            .HasName("pk_c_language_level");

        entity.ToTable("c_language_level");

        entity.Property(e => e.LanguageLevelId).HasColumnName("language_level_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_language_level_enum_key").IsUnique();

        entity.HasData(CLanguageLevelSeeder.Entities);
    }
}