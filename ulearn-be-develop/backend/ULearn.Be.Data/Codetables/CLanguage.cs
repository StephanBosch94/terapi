using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CLanguage : CodetableBase
    {
        public CLanguage()
        {
            RUserLanguages = new HashSet<RUserLanguage>();
        }

        public int LanguageId { get; set; }
        public virtual ICollection<RUserLanguage> RUserLanguages { get; set; }
    }
}

public sealed class CLanguageConfiguration : IEntityTypeConfiguration<CLanguage>
{
    public void Configure(EntityTypeBuilder<CLanguage> entity)
    {
        entity.HasKey(e => e.LanguageId)
            .HasName("pk_c_language");

        entity.ToTable("c_language");

        entity.Property(e => e.LanguageId).HasColumnName("language_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_language_enum_key").IsUnique();

        entity.HasData(CLanguageSeeder.Entities);
    }
}