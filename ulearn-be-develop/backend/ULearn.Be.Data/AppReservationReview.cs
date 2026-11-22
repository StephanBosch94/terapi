using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppReservationReview
{
    [Column("reservation_review_id")] public int ReservationReviewId { get; set; }
    [Column("reservation_id")] public int ReservationId { get; set; }
    [Column("creator_id")] public int CreatorId { get; set; }
    [Column("value")] public int Value { get; set; }
    [Column("text_value")] public string TextValue { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("updated")] public DateTime Updated { get; set; }
    public virtual AppReservation Reservation { get; set; }
    public virtual AppUser Creator { get; set; }
}

public sealed class AppReservationReviewConfiguration : IEntityTypeConfiguration<AppReservationReview>
{
    public void Configure(EntityTypeBuilder<AppReservationReview> entity)
    {
        entity.HasKey(e => e.ReservationReviewId)
            .HasName("pk_app_reservation_review");

        entity.ToTable("app_reservation_review");

        entity.HasIndex(e => e.ReservationId, "ix_app_reservation_review_reservation_id");
        entity.HasIndex(e => e.CreatorId, "ix_app_reservation_review_creator_id");

        entity.HasIndex(e => new { e.ReservationId, e.CreatorId },
            "uc_app_reservation_review_reservation_id_creator_id").IsUnique();

        entity.HasOne(e => e.Reservation)
            .WithMany(e => e.AppReservationReviews)
            .HasForeignKey(e => e.ReservationId)
            .HasConstraintName("fk_app_reservation_review_app_reservation_reservation_id");

        entity.HasOne(e => e.Creator)
            .WithMany(e => e.AppReservationReviews)
            .HasForeignKey(e => e.CreatorId)
            .HasConstraintName("fk_app_reservation_review_app_user_creator_id");
    }
}