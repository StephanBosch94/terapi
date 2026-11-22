using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CSpecialization : CodetableBase
{
    [Column("specialization_id")] public int SpecializationId { get; set; }
    [Column("specialization_category_id")] public int SpecializationCategoryId { get; set; }
    [Column("subject_id")] public int SubjectId { get; set; }
    [Column("icon")] public string Icon { get; set; }

    [Column("hero_title_key")]
    [MaxLength(100)]
    public string HeroTitleKey { get; set; }

    [Column("hero_description_key")]
    [MaxLength(100)]
    public string HeroDescriptionKey { get; set; }

    [Column("page_title_key")]
    [MaxLength(100)]
    public string PageTitleKey { get; set; }

    [Column("page_description_key")]
    [MaxLength(100)]
    public string PageDescriptionKey { get; set; }

    [Column("is_lesson_for_children_type")]
    public bool IsLessonForChildrenType { get; set; }

    public CSubject CSubject { get; set; }
    public CSpecializationCategory CSpecializationCategory { get; set; }
    public ICollection<RUserSpecialization> RUserSpecializations { get; set; } = new List<RUserSpecialization>();
}

public sealed class CSpecializationConfiguration : IEntityTypeConfiguration<CSpecialization>
{
    public void Configure(EntityTypeBuilder<CSpecialization> entity)
    {
        entity.HasKey(e => e.SpecializationId)
            .HasName("pk_specialization");

        entity.ToTable("c_specialization");

        entity.HasOne(e => e.CSubject)
            .WithMany(e => e.CSpecializations)
            .HasForeignKey(e => e.SubjectId)
            .HasConstraintName("fk_c_specialization_c_subject_subject_id");

        entity.HasOne(e => e.CSpecializationCategory)
            .WithMany(e => e.CSpecializations)
            .HasForeignKey(e => e.SpecializationCategoryId)
            .HasConstraintName("fk_c_specialization_c_specialization_category_specialization_category_id");

        entity.HasData(CSpecializationSeeder.Entities);
    }
}