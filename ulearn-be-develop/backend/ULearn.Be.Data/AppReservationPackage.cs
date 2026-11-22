using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppReservationPackage
{
    [Column("reservation_package_id")] public int ReservationPackageId { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("cancelled")] public DateTime? Cancelled { get; set; }
    [Column("host_id")] public int HostId { get; set; }
    [Column("attender_id")] public int AttenderId { get; set; }
    [Column("cancellation_reason")] public string CancellationReason { get; set; }

    [Column("reservation_package_state_id")]
    public int ReservationPackageStateId { get; set; }

    [Column("total_price")] public double TotalPrice { get; set; }
    [Column("currency_id")] public int CurrencyId { get; set; }
    [Column("class_duration_id")] public int ClassDurationId { get; set; }
    public AppUser Attender { get; set; }
    public AppUser Host { get; set; }
    public CReservationPackageState CReservationPackageState { get; set; }
    public CCurrency Currency { get; set; }
    public CClassDuration CClassDuration { get; set; }
    public ICollection<AppReservation> AppReservations { get; set; }
    public ICollection<AppTransaction> AppTransactions { get; set; }
}

public sealed class AppReservationPackageConfiguration : IEntityTypeConfiguration<AppReservationPackage>
{
    public void Configure(EntityTypeBuilder<AppReservationPackage> entity)
    {
        entity.HasKey(e => e.ReservationPackageId)
            .HasName("pk_app_reservation_package");

        entity.ToTable("app_reservation_package");

        entity.HasIndex(e => e.AttenderId, "ix_app_reservation_package_attender_id");
        entity.HasIndex(e => e.HostId, "ix_app_reservation_package_host_id");
        entity.HasIndex(e => e.ReservationPackageStateId, "ix_app_reservation_package_reservation_package_state_id");
        entity.HasIndex(e => e.CurrencyId, "ix_app_reservation_package_currency_id");
        entity.HasIndex(e => e.ClassDurationId, "ix_app_reservation_package_class_duration_id");

        entity.HasOne(e => e.Attender)
            .WithMany(e => e.AttenderReservationPackages)
            .HasForeignKey(e => e.AttenderId)
            .HasConstraintName("fk_app_reservation_package_app_user_attender_id");

        entity.HasOne(e => e.Host)
            .WithMany(e => e.HostReservationPackages)
            .HasForeignKey(e => e.HostId)
            .HasConstraintName("fk_app_reservation_package_app_user_host_id");

        entity.HasOne(e => e.CReservationPackageState)
            .WithMany(e => e.AppReservationPackages)
            .HasForeignKey(e => e.ReservationPackageStateId)
            .HasConstraintName("fk_app_reservation_package_c_reservation_package_state_reservation_package_state_id");

        entity.HasOne(e => e.Currency)
            .WithMany(e => e.AppReservationPackages)
            .HasForeignKey(e => e.CurrencyId)
            .HasConstraintName("fk_app_reservation_package_c_currency_currency_id");

        entity.HasOne(e => e.CClassDuration)
            .WithMany(e => e.AppReservationPackages)
            .HasForeignKey(e => e.ClassDurationId)
            .HasConstraintName("fk_app_reservation_package_c_class_duration_class_duration_id");
    }
}