using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppRoom
{
    public int RoomId { get; set; }
    public string RoomKey { get; set; }
    public int RoomStateId { get; set; }
    public int HostId { get; set; }
    public int AttenderId { get; set; }
    public DateTime ScheduledFromTime { get; set; }
    public DateTime ScheduledToTime { get; set; }
    public virtual CRoomState CRoomState { get; set; }
    public virtual AppUser Host { get; set; }
    public virtual AppUser Attender { get; set; }
    public virtual ICollection<AppRoomActivity> AppRoomActivities { get; set; }
    public virtual ICollection<RUserRoomStatistic> RUserRoomStatistics { get; set; }
}

public sealed class AppRoomConfiguration : IEntityTypeConfiguration<AppRoom>
{
    public void Configure(EntityTypeBuilder<AppRoom> entity)
    {
        entity.HasKey(e => e.RoomId)
            .HasName("pk_app_room");

        entity.ToTable("app_room");
        entity.Property(e => e.RoomId).HasColumnName("room_id");
        entity.Property(e => e.RoomKey).HasColumnName("room_key");
        entity.Property(e => e.RoomStateId).HasColumnName("room_state_id");
        entity.Property(e => e.ScheduledFromTime).HasColumnName("scheduled_from_time");
        entity.Property(e => e.ScheduledToTime).HasColumnName("scheduled_to_time");
        entity.Property(e => e.HostId).HasColumnName("host_id");
        entity.Property(e => e.AttenderId).HasColumnName("attender_id");

        entity.HasIndex(e => e.RoomStateId, "ix_app_room_room_state_id");
        entity.HasIndex(e => e.HostId, "ix_app_room_room_host_id");
        entity.HasIndex(e => e.AttenderId, "ix_app_room_room_attender_id");

        entity.HasOne(e => e.CRoomState)
            .WithMany(e => e.AppRooms)
            .HasForeignKey(e => e.RoomStateId)
            .HasConstraintName("fk_app_room_c_room_state_room_state_id");

        entity.HasOne(e => e.Host)
            .WithMany(x => x.HostAppRooms)
            .HasForeignKey(e => e.HostId)
            .HasConstraintName("fk_app_room_app_user_host_id");

        entity.HasOne(e => e.Attender)
            .WithMany(x => x.AttenderAppRooms)
            .HasForeignKey(e => e.AttenderId)
            .HasConstraintName("fk_app_room_app_attender_attender_id");
    }
}