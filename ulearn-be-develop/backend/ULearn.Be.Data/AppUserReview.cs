using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;

namespace ULearn.Be.Data
{
    public partial class AppUserReview
    {
        public int UserReviewId { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual AppUser Reviewee { get; set; } = null!;
        public virtual AppUser Reviewer { get; set; } = null!;
    }
}

public sealed class AppUserReviewConfiguration : IEntityTypeConfiguration<AppUserReview>
{
    public void Configure(EntityTypeBuilder<AppUserReview> entity)
    {
        entity.HasKey(e => e.UserReviewId)
            .HasName("pk_app_user_review");

        entity.ToTable("app_user_review");

        entity.HasIndex(e => e.RevieweeId, "ix_app_user_review_reviewee_id");

        entity.HasIndex(e => e.ReviewerId, "ix_app_user_review_reviewer_id");

        entity.Property(e => e.UserReviewId).HasColumnName("user_review_id");

        entity.Property(e => e.Comment)
            .HasMaxLength(1000)
            .HasColumnName("comment");

        entity.Property(e => e.Created)
            .HasColumnName("created");

        entity.Property(e => e.Rate).HasColumnName("rate");

        entity.Property(e => e.RevieweeId).HasColumnName("reviewee_id");

        entity.Property(e => e.ReviewerId).HasColumnName("reviewer_id");

        entity.Property(e => e.Updated)
            .HasColumnName("updated");

        entity.HasOne(d => d.Reviewee)
            .WithMany(p => p.AppUserReviewReviewees)
            .HasForeignKey(d => d.RevieweeId)
            .HasConstraintName("fk_app_user_review_app_user_reviewee_id");

        entity.HasOne(d => d.Reviewer)
            .WithMany(p => p.AppUserReviewReviewers)
            .HasForeignKey(d => d.ReviewerId)
            .HasConstraintName("fk_app_user_review_app_user_reviewer_id");
    }
}