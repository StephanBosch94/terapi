using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CTeachingExperience : CodetableBase
{
    public CTeachingExperience()
    {
        AppUsers = new HashSet<AppUser>();
    }

    public int TeachingExperienceId { get; set; }
    public virtual ICollection<AppUser> AppUsers { get; set; }
}

public sealed class CTeachingExperienceConfiguration : IEntityTypeConfiguration<CTeachingExperience>
{
    public void Configure(EntityTypeBuilder<CTeachingExperience> entity)
    {
        entity.HasKey(e => e.TeachingExperienceId)
            .HasName("pk_c_teaching_experience");

        entity.ToTable("c_teaching_experience");

        entity.Property(e => e.TeachingExperienceId).HasColumnName("teaching_experience_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_teaching_experience_enum_key").IsUnique();
        
        entity.HasData(CTeachingExperienceSeeder.Entities);
    }
}