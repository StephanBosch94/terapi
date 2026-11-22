using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CNotificationState : CodetableBase
{
    [EnumKey("notification_state_id")] public int NotificationStateId { get; set; }
    public virtual ICollection<AppNotification> AppNotifications { get; set; } = new List<AppNotification>();
    public virtual ICollection<RUserNotification> RUserNotifications { get; set; } = new List<RUserNotification>();
}

public sealed class CNotificationStateConfiguration : IEntityTypeConfiguration<CNotificationState>
{
    public void Configure(EntityTypeBuilder<CNotificationState> entity)
    {
        entity.HasKey(e => e.NotificationStateId)
            .HasName("pk_c_notification_state");

        entity.ToTable("c_notification_state");

        entity.HasIndex(e => e.EnumKey, "ix_c_notification_state_enum_key").IsUnique();

        entity.HasData(CNotificationStateSeeder.Entities);
    }
}