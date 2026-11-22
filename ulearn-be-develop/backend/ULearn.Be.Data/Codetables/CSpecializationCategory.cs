using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CSpecializationCategory : CodetableBase
{
    [Column("specialization_category_id")] public int SpecializationCategoryId { get; set; }
    public ICollection<CSpecialization> CSpecializations { get; set; } = new List<CSpecialization>();
}

public sealed class CSpecializationCategoryConfiguration : IEntityTypeConfiguration<CSpecializationCategory>
{
    public void Configure(EntityTypeBuilder<CSpecializationCategory> entity)
    {
        entity.HasKey(e => e.SpecializationCategoryId)
            .HasName("pk_c_specialization_category");

        entity.ToTable("c_specialization_category");

        entity.HasData(CSpecializationCategorySeeder.Entities);
    }
}