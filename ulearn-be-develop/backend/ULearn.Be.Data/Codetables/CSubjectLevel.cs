using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CSubjectLevel
{
    [Column("enum_key")] [MaxLength(100)] public string EnumKey { get; set; } = null!;

    [Column("tutor_name_key")]
    [MaxLength(100)]
    public string TutorNameKey { get; set; } = null!;

    [Column("student_name_key")]
    [MaxLength(100)]
    public string StudentNameKey { get; set; } = null!;

    [Column("description_key")]
    [MaxLength(100)]
    public string DescriptionKey { get; set; } = null!;

    [Column("icon_name")] [MaxLength(100)] public string IconName { get; set; }

    [Column("inverted_icon_name")]
    [MaxLength(100)]
    public string InvertedIconName { get; set; }

    [Column("subject_level_id")] public int SubjectLevelId { get; set; }


    public virtual ICollection<RUserSubjectLevel> RUserSubjectLevels { get; set; }
}

public sealed class CSubjectLevelConfiguration : IEntityTypeConfiguration<CSubjectLevel>
{
    public void Configure(EntityTypeBuilder<CSubjectLevel> entity)
    {
        entity.HasKey(e => e.SubjectLevelId)
            .HasName("pk_c_subject_level");

        entity.ToTable("c_subject_level");

        entity.HasIndex(e => e.EnumKey, "ix_c_subject_level_enum_key").IsUnique();

        entity.HasData(CSubjectLevelSeeder.Entities);
    }
}