using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CRoomState: CodetableBase
{
    public int RoomStateId { get; set; }
    public virtual ICollection<AppRoom> AppRooms { get; set; }
}

public sealed class CRoomStateConfiguration : IEntityTypeConfiguration<CRoomState>
{
    public void Configure(EntityTypeBuilder<CRoomState> entity)
    {
        entity.HasKey(e => e.RoomStateId)
            .HasName("pk_c_room_state");

        entity.ToTable("c_room_state");

        entity.Property(e => e.RoomStateId).HasColumnName("room_state_id");

        entity.HasData(CRoomStateSeeder.Entities);
    }
}