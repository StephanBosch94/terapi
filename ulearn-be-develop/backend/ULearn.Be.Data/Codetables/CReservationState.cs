using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CReservationState : CodetableBase
    {
        public int ReservationStateId { get; set; }
        public virtual ICollection<AppReservation> AppReservations { get; set; }
    }
}

public sealed class CReservationStateConfiguration : IEntityTypeConfiguration<CReservationState>
{
    public void Configure(EntityTypeBuilder<CReservationState> entity)
    {
        entity.HasKey(e => e.ReservationStateId)
            .HasName("pk_c_reservation_state");

        entity.ToTable("c_reservation_state");

        entity.Property(e => e.ReservationStateId).HasColumnName("reservation_state_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_reservation_state_enum_key").IsUnique();

        entity.HasData(CReservationStateSeeder.Entities);
    }
}