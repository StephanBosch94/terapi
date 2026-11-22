using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CCategory : CodetableBase
    {
        public CCategory()
        {
            CSubjects = new HashSet<CSubject>();
        }

        public int CategoryId { get; set; }
        public virtual ICollection<CSubject> CSubjects { get; set; }
    }
    
    public sealed class CCategoryConfiguration : IEntityTypeConfiguration<CCategory>
    {
        public void Configure(EntityTypeBuilder<CCategory> entity)
        {
            entity.HasKey(e => e.CategoryId)
                .HasName("pk_c_category");

            entity.ToTable("c_category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasIndex(e => e.EnumKey, "ix_c_category_enum_key").IsUnique();
            
            entity.HasData(CCategorySeeder.Entities);
        }
    }
}