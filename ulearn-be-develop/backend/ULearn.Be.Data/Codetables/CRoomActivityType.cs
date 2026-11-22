using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CRoomActivityType: CodetableBase
{
    public int RoomActivityTypeId { get; set; }

    public virtual ICollection<AppRoomActivity> AppRoomActivities { get; set; }
}

public sealed class CRoomActivityTypeConfiguration : IEntityTypeConfiguration<CRoomActivityType>
{
    public void Configure(EntityTypeBuilder<CRoomActivityType> entity)
    {
        entity.HasKey(e => e.RoomActivityTypeId)
            .HasName("pk_c_room_activity_type");

        entity.ToTable("c_room_activity_type");

        entity.Property(e => e.RoomActivityTypeId).HasColumnName("room_activity_type_id");

        entity.HasData(CRoomActivitySeeder.Entities);
    }
}