using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CReservationPackageState: CodetableBase
{
    public int ReservationStateId { get; set; }
    public virtual ICollection<AppReservationPackage> AppReservationPackages { get; set; }
}

public sealed class CReservationPackageStateConfiguration : IEntityTypeConfiguration<CReservationPackageState>
{
    public void Configure(EntityTypeBuilder<CReservationPackageState> entity)
    {
        entity.HasKey(e => e.ReservationStateId)
            .HasName("pk_c_reservation_package_state");

        entity.ToTable("c_reservation_package_state");

        entity.Property(e => e.ReservationStateId).HasColumnName("reservation_state_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_reservation_package_state_enum_key").IsUnique();

        entity.HasData(CReservationPackageStateSeeder.Entities);
    }
}