using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CNotificationChannel : CodetableBase
{
    [Column("notification_channel_id")] public int NotificationChannelId { get; set; }
    public ICollection<RUserNotification> RUserNotifications { get; set; } = new List<RUserNotification>();
    public virtual ICollection<RUserNotificationBlacklistSetting> RUserNotificationBlacklistSettings { get; set; } = new List<RUserNotificationBlacklistSetting>();
}

public sealed class CNotificationChannelConfiguration : IEntityTypeConfiguration<CNotificationChannel>
{
    public void Configure(EntityTypeBuilder<CNotificationChannel> entity)
    {
        entity.HasKey(e => e.NotificationChannelId)
            .HasName("pk_c_notification_channel");

        entity.ToTable("c_notification_channel");

        entity.HasIndex(e => e.EnumKey, "ix_c_notification_channel_enum_key").IsUnique();

        entity.HasData(CNotificationChannelSeeder.Entities);
    }
}