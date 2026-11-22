using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class RUserNotificationBlacklistSetting
{
    [Column("user_notification_blacklist_setting_id")] public int UserNotificationBlacklistSettingId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("notification_type_id")] public int NotificationTypeId { get; set; }
    [Column("notification_channel_id")] public int NotificationChannelId { get; set; }
    public AppUser AppUser { get; set; }
    public CNotificationType CNotificationType { get; set; }
    public CNotificationChannel CNotificationChannel { get; set; }
}

public sealed class RUserNotificationBlacklistSettingConfiguration : IEntityTypeConfiguration<RUserNotificationBlacklistSetting>
{
    public void Configure(EntityTypeBuilder<RUserNotificationBlacklistSetting> entity)
    {
        entity.HasKey(e => e.UserNotificationBlacklistSettingId)
            .HasName("pk_r_user_notification_blacklist_setting");

        entity.ToTable("r_user_notification_blacklist_setting");

        entity.HasIndex(e => e.UserId, "ix_r_user_notification_blacklist_setting_user_id");
        entity.HasIndex(e => e.NotificationChannelId, "ix_r_user_notification_blacklist_setting_notification_channel_id");
        entity.HasIndex(e => e.NotificationTypeId, "ix_r_user_notification_blacklist_setting_notification_type_id");

        entity.HasOne(e => e.AppUser)
            .WithMany(e => e.RUserNotificationBlacklistSettings)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_r_user_notification_blacklist_setting_app_user_user_id");

        entity.HasOne(e => e.CNotificationChannel)
            .WithMany(e => e.RUserNotificationBlacklistSettings)
            .HasForeignKey(e => e.NotificationChannelId)
            .HasConstraintName("fk_r_user_notification_blacklist_setting_c_notification_channel_notification_channel_id");

        entity.HasOne(e => e.CNotificationType)
            .WithMany(e => e.RUserNotificationBlacklistSettings)
            .HasForeignKey(e => e.NotificationTypeId)
            .HasConstraintName("fk_r_user_notification_blacklist_setting_c_notification_type_notification_type_id");
    }
}