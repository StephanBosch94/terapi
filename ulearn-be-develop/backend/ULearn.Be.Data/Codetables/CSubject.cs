using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CSubject : CodetableBase
    {
        [Column("subject_id")] public int SubjectId { get; set; }
        [Column("icon_name")] [MaxLength(100)] public string IconName { get; set; }

        [Column("hero_title_key")]
        [MaxLength(100)]
        public string HeroTitleKey { get; set; }

        [Column("hero_description_key")]
        [MaxLength(100)]
        public string HeroDescriptionKey { get; set; }

        [Column("page_title_key")]
        [MaxLength(100)]
        public string PageTitleKey { get; set; }

        [Column("footer_subject_teachers_page_link_key")]
        [MaxLength(100)]
        public string FooterSubjectTeachersPageLinkKey { get; set; }

        [Column("footer_subject_landing_page_link_key")]
        [MaxLength(100)]
        public string FooterSubjectLandingPageLinkKey { get; set; }

        [Column("user_goal_title_key")]
        [MaxLength(100)]
        public string UserGoalTitleKey { get; set; }

        [Column("tutor_public_page_title_key")]
        [MaxLength(100)]
        public string TutorPublicPageTitleKey { get; set; }

        [Column("tutor_public_page_description_key")]
        [MaxLength(100)]
        public string TutorPublicPageDescriptionKey { get; set; }

        [Column("category_id")] public int CategoryId { get; set; }
        [Column("object_state_id")] public int ObjectStateId { get; set; }

        public CCategory Category { get; set; } = null!;
        public CObjectState CObjectState { get; set; } = null!;

        public ICollection<CSpecialization> CSpecializations { get; set; } = new List<CSpecialization>();
        public ICollection<AppReservation> AppReservations { get; set; } = new List<AppReservation>();
        public ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
    }
}

public sealed class CSubjectConfiguration : IEntityTypeConfiguration<CSubject>
{
    public void Configure(EntityTypeBuilder<CSubject> entity)
    {
        entity.HasKey(e => e.SubjectId)
            .HasName("pk_c_subject");

        entity.ToTable("c_subject");

        entity.HasIndex(e => e.CategoryId, "ix_c_subject_category_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_subject_enum_key").IsUnique();

        entity.HasOne(d => d.Category)
            .WithMany(p => p.CSubjects)
            .HasForeignKey(d => d.CategoryId)
            .HasConstraintName("fk_c_subject_c_category_category_id");

        entity.HasOne(d => d.CObjectState)
            .WithMany(p => p.CSubjects)
            .HasForeignKey(d => d.ObjectStateId)
            .HasConstraintName("fk_c_subject_c_object_state_object_state_id");

        entity.HasData(CSubjectSeeder.Entities);
    }
}