using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppNotification
{
    [Column("notification_id")] public int NotificationId { get; set; }
    [Column("recipient_id")] public int RecipientId { get; set; }
    [Column("sender_id")] public int SenderId { get; set; }
    [Column("notification_type_id")] public int NotificationTypeId { get; set; }
    [Column("notification_state_id")] public int NotificationStateId { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("read_date_time")] public DateTime? ReadDateTime { get; set; }
    [Column("json_data")] public string JsonData { get; set; }
    public AppUser Recipient { get; set; }
    public AppUser Sender { get; set; }
    public CNotificationType CNotificationType { get; set; }
    public CNotificationState CNotificationState { get; set; }
}

public sealed class AppNotificationConfiguration : IEntityTypeConfiguration<AppNotification>
{
    public void Configure(EntityTypeBuilder<AppNotification> entity)
    {
        entity.HasKey(e => e.NotificationId)
            .HasName("pk_app_notification");

        entity.ToTable("app_notification");

        entity.HasIndex(e => e.RecipientId, "ix_app_notification_recipient_id");
        entity.HasIndex(e => e.SenderId, "ix_app_notification_sender_id");
        entity.HasIndex(e => e.NotificationTypeId, "ix_app_notification_notification_type_id");
        entity.HasIndex(e => e.NotificationStateId, "ix_app_notification_notification_state_id");

        entity.HasOne(d => d.Recipient)
            .WithMany(p => p.OwnedAppNotifications)
            .HasForeignKey(d => d.RecipientId)
            .HasConstraintName("fk_app_notification_app_user_recipient_id");

        entity.HasOne(d => d.Sender)
            .WithMany(p => p.SentAppNotifications)
            .HasForeignKey(d => d.SenderId)
            .HasConstraintName("fk_app_notification_app_user_sender_id");

        entity.HasOne(d => d.CNotificationType)
            .WithMany(p => p.AppNotifications)
            .HasForeignKey(d => d.NotificationTypeId)
            .HasConstraintName("fk_app_notification_c_notification_type_notification_type_id");

        entity.HasOne(d => d.CNotificationState)
            .WithMany(p => p.AppNotifications)
            .HasForeignKey(d => d.NotificationStateId)
            .HasConstraintName("fk_app_notification_c_notification_state_notification_state_id");
    }
}