using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;

namespace ULearn.Be.Data
{
    public partial class AppReservation : ScheduledEntity
    {
        [Column("reservation_id")] public int ReservationId { get; set; }
        [Column("reservation_package_id")] public int ReservationPackageId { get; set; }
        [Column("subject_id")] public int SubjectId { get; set; }
        [Column("host_id")] public int HostId { get; set; }
        [Column("attender_id")] public int AttenderId { get; set; }
        [Column("reservation_state_id")] public int ReservationStateId { get; set; }
        [Column("price")] public double Price { get; set; }
        [Column("currency_id")] public int CurrencyId { get; set; }
        [Column("price_without_sales")] public double PriceWithoutSales { get; set; }
        [Column("created")] public DateTime Created { get; set; }
        [Column("updated")] public DateTime? Updated { get; set; }
        [Column("notified")] public DateTime? Notified { get; set; }
        [Column("room_id")] public int RoomId { get; set; }
        [Column("host_event_iana_timezone")] public string HostEventIanaTimezone { get; set; }
        [Column("source_reservation_id")] public int? SourceReservationId { get; set; }
        [Column("class_duration_id")] public int ClassDurationId { get; set; }
        [Column("expected_end_time")] public DateTime ExpectedEndTime { get; set; }
        [Column("coupon_id")] public int? CouponId { get; set; }
        [Column("coupon_sale_amount")] public double? CouponSaleAmount { get; set; }
        public virtual AppUser Host { get; set; } = null!;
        public virtual AppReservationPackage AppReservationPackage { get; set; } = null!;
        public virtual AppUser Attender { get; set; } = null!;
        public virtual CReservationState ReservationState { get; set; } = null!;
        public virtual CSubject Subject { get; set; } = null!;
        public virtual AppRoom AppRoom { get; set; } = null!;
        public virtual CCurrency Currency { get; set; } = null!;
        public virtual CClassDuration CClassDuration { get; set; }
        public virtual AppReservation SourceReservation { get; set; } = null!;
        public virtual AppCoupon AppCoupon { get; set; } = null!;
        public virtual ICollection<AppReservationReview> AppReservationReviews { get; set; } = null!;
        public virtual ICollection<AppReservationIssue> AppReservationIssues { get; set; } = null!;
        public virtual ICollection<AppTransaction> AppTransactions { get; set; } = null!;
        public virtual ICollection<AppReservation> ChildrenReservations { get; set; } = null!;
    }
}

public sealed class AppReservationConfiguration : IEntityTypeConfiguration<AppReservation>
{
    public void Configure(EntityTypeBuilder<AppReservation> entity)
    {
        entity.HasKey(e => e.ReservationId)
            .HasName("pk_app_reservation");

        entity.ToTable("app_reservation");

        entity.HasIndex(e => e.ReservationPackageId, "ix_app_reservation_reservation_package_id");
        entity.HasIndex(e => e.AttenderId, "ix_app_reservation_attender_id");
        entity.HasIndex(e => e.HostId, "ix_app_reservation_host_id");
        entity.HasIndex(e => e.ReservationStateId, "ix_app_reservation_reservation_state_id");
        entity.HasIndex(e => e.SubjectId, "ix_app_reservation_subject_id");
        entity.HasIndex(e => e.RoomId, "ix_app_reservation_room_id");
        entity.HasIndex(e => e.CurrencyId, "ix_app_reservation_currency_id");
        entity.HasIndex(e => e.ReservationId, "ix_app_reservation_source_reservation_id");
        entity.HasIndex(e => e.ClassDurationId, "ix_app_reservation_class_duration_id");
        entity.HasIndex(e => e.CouponId, "ix_app_reservation_coupon_id");
        entity.HasIndex(
            e => new { e.HostId, e.AttenderId, e.StartTime, e.EndTime },
            "ix_app_reservation_host_id_attender_id_start_local_time_end_local_time");

        entity.HasOne(d => d.Attender)
            .WithMany(p => p.AttenderAppReservations)
            .HasForeignKey(d => d.AttenderId)
            .HasConstraintName("fk_app_reservation_app_user_attender_id");

        entity.HasOne(d => d.Host)
            .WithMany(p => p.HostAppReservations)
            .HasForeignKey(d => d.HostId)
            .HasConstraintName("fk_app_reservation_app_user_host_id");

        entity.HasOne(d => d.ReservationState)
            .WithMany(p => p.AppReservations)
            .HasForeignKey(d => d.ReservationStateId)
            .HasConstraintName("fk_app_reservation_c_reservation_state_reservation_state_id");

        entity.HasOne(d => d.Subject)
            .WithMany(p => p.AppReservations)
            .HasForeignKey(d => d.SubjectId)
            .HasConstraintName("fk_app_reservation_c_subject_subject_id");

        entity.HasOne(e => e.AppRoom)
            .WithMany()
            .HasForeignKey(e => e.RoomId)
            .HasConstraintName("fk_app_reservation_app_room_room_id");

        entity.HasOne(e => e.AppReservationPackage)
            .WithMany(e => e.AppReservations)
            .HasForeignKey(e => e.ReservationPackageId)
            .HasConstraintName("fk_app_reservation_app_reservation_package_reservation_package_id");

        entity.HasOne(e => e.Currency)
            .WithMany(e => e.AppReservations)
            .HasForeignKey(e => e.CurrencyId)
            .HasConstraintName("fk_app_reservation_c_currency_currency_id");

        entity.HasOne(e => e.SourceReservation)
            .WithMany(e => e.ChildrenReservations)
            .HasForeignKey(e => e.SourceReservationId)
            .HasConstraintName("fk_app_reservation_app_reservation_source_reservation_id");

        entity.HasOne(e => e.CClassDuration)
            .WithMany(e => e.AppReservations)
            .HasForeignKey(e => e.ClassDurationId)
            .HasConstraintName("fk_app_reservation_c_class_duration_class_duration_id");

        entity.HasOne(e => e.AppCoupon)
            .WithMany(e => e.AppReservations)
            .HasForeignKey(e => e.CouponId)
            .HasConstraintName("fk_app_reservation_app_coupon_coupon_id");
    }
}