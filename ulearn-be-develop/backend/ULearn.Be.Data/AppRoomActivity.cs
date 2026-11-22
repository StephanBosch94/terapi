using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppRoomActivity
{
    public int RoomActivityId { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public int RoomActivityTypeId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public virtual CRoomActivityType CRoomActivityType { get; set; }
    public virtual AppRoom AppRoom { get; set; }
    public virtual AppUser AppUser { get; set; }
}

public sealed class AppRoomActivityConfiguration : IEntityTypeConfiguration<AppRoomActivity>
{
    public void Configure(EntityTypeBuilder<AppRoomActivity> entity)
    {
        entity.HasKey(e => e.RoomActivityId)
            .HasName("pk_app_room_activity");

        entity.ToTable("app_room_activity");
        entity.Property(e => e.RoomActivityId).HasColumnName("room_activity_id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.RoomId).HasColumnName("room_id");
        entity.Property(e => e.RoomActivityTypeId).HasColumnName("room_activity_type_id");
        entity.Property(e => e.Created).HasColumnName("created");
        entity.Property(e => e.Updated).HasColumnName("updated");

        entity.HasIndex(e => e.UserId, "ix_app_room_activity_user_id");
        entity.HasIndex(e => e.RoomId, "ix_app_room_activity_room_id");
        entity.HasIndex(e => e.RoomActivityTypeId, "ix_app_room_activity_room_activity_id");
        entity.HasIndex(e => new { e.UserId, e.RoomId, e.Created },
            "ix_app_room_activity_user_id_room_id_created");

        entity.HasOne(e => e.CRoomActivityType)
            .WithMany(x => x.AppRoomActivities)
            .HasForeignKey(e => e.RoomActivityTypeId)
            .HasConstraintName("fk_app_room_activity_c_room_activity_type_room_activity_type_id");

        entity.HasOne(e => e.AppRoom)
            .WithMany(x => x.AppRoomActivities)
            .HasForeignKey(e => e.RoomId)
            .HasConstraintName("fk_app_room_activity_app_room_room_id");

        entity.HasOne(e => e.AppUser)
            .WithMany(x => x.AppRoomActivities)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_app_room_activity_app_user_user_id");
    }
}