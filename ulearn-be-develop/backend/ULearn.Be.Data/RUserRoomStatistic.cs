using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class RUserRoomStatistic
{
    public int UserRoomStatisticId { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }
    public int RoomStatisticsTypeId { get; set; }
    public DateTime Created { get; set; }
    public double Value { get; set; }
    public string TextValue { get; set; }

    public AppRoom AppRoom { get; set; }
    public AppUser AppUser { get; set; }
    public CRoomStatisticType CRoomStatisticType { get; set; }
}

public sealed class RUserRoomStatisticConfiguration : IEntityTypeConfiguration<RUserRoomStatistic>
{
    public void Configure(EntityTypeBuilder<RUserRoomStatistic> entity)
    {
        entity.HasKey(e => e.UserRoomStatisticId)
            .HasName("pk_r_user_room_statistic");

        entity.ToTable("r_user_room_statistic");

        entity.HasIndex(e => e.RoomId, "ix_r_user_room_statistic_room_id");

        entity.HasIndex(e => e.UserId, "ix_r_user_room_statistic_user_id");

        entity.HasIndex(e => e.RoomStatisticsTypeId, "ix_r_user_room_statistic_room_statistics_type_id");

        entity.Property(e => e.UserRoomStatisticId).HasColumnName("user_room_statistic_id");

        entity.Property(e => e.RoomId).HasColumnName("room_id");

        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.Property(e => e.RoomStatisticsTypeId).HasColumnName("room_statistics_type_id");

        entity.Property(e => e.Created).HasColumnName("created");

        entity.Property(e => e.Value).HasColumnName("value");

        entity.Property(e => e.TextValue).HasColumnName("text_value");

        entity.HasOne(d => d.AppRoom)
            .WithMany(p => p.RUserRoomStatistics)
            .HasForeignKey(d => d.RoomId)
            .HasConstraintName("fk_r_user_room_statistic_app_room_room_id");

        entity.HasOne(d => d.AppUser)
            .WithMany(p => p.RUserRoomStatistics)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_room_statistic_app_user_user_id");

        entity.HasOne(d => d.CRoomStatisticType)
            .WithMany(p => p.RUserRoomStatistics)
            .HasForeignKey(d => d.RoomStatisticsTypeId)
            .HasConstraintName("fk_r_user_room_statistic_c_room_statistic_type_room_statistic_type_id");
    }
}