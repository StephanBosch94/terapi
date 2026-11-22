using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class RUserNotification
{
    [Column("User_Notification_Id")] public int UserNotificationId { get; set; }
    [Column("User_Id")] public int UserId { get; set; }
    [Column("Notification_Type_Id")] public int NotificationTypeId { get; set; }
    [Column("Notification_Channel_Id")] public int NotificationChannelId { get; set; }
    [Column("Notification_State_Id")] public int NotificationStateId { get; set; }
    [Column("Created")] public DateTime Created { get; set; }

    public AppUser AppUser { get; set; }
    public CNotificationChannel CNotificationChannel { get; set; }
    public CNotificationType CNotificationType { get; set; }
    public CNotificationState CNotificationState { get; set; }
}

public sealed class RUserNotificationConfiguration : IEntityTypeConfiguration<RUserNotification>
{
    public void Configure(EntityTypeBuilder<RUserNotification> entity)
    {
        entity.HasKey(e => e.UserNotificationId)
            .HasName("pk_r_user_notification");

        entity.ToTable("r_user_notification");

        entity.HasIndex(e => e.UserId, "ix_r_user_notification_user_id");
        entity.HasIndex(e => e.NotificationChannelId, "ix_r_user_notification_notification_channel_id");
        entity.HasIndex(e => e.NotificationTypeId, "ix_r_user_notification_notification_type_id");
        entity.HasIndex(e => e.NotificationStateId, "ix_r_user_notification_notification_state_id");

        entity.HasOne(e => e.AppUser)
            .WithMany(e => e.RUserNotifications)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_r_user_notification_app_user_user_id");

        entity.HasOne(e => e.CNotificationChannel)
            .WithMany(e => e.RUserNotifications)
            .HasForeignKey(e => e.NotificationChannelId)
            .HasConstraintName("fk_r_user_notification_c_notification_channel_notification_channel_id");

        entity.HasOne(e => e.CNotificationType)
            .WithMany(e => e.RUserNotifications)
            .HasForeignKey(e => e.NotificationTypeId)
            .HasConstraintName("fk_r_user_notification_c_notification_type_notification_type_id");

        entity.HasOne(e => e.CNotificationState)
            .WithMany(e => e.RUserNotifications)
            .HasForeignKey(e => e.NotificationStateId)
            .HasConstraintName("fk_r_user_notification_c_notification_state_notification_state_id");
    }
}