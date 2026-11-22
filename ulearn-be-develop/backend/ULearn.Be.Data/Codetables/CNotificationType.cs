using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CNotificationType : CodetableBase
{
    [Column("notification_type_id")] public int NotificationTypeId { get; set; }
    [Column("title_key")] public string TitleKey { get; set; }
    [Column("setting_title_key")] public string SettingTitleKey { get; set; }
    [Column("setting_description_key")] public string SettingDescriptionKey { get; set; }
    public virtual ICollection<RNotificationTypeUserRole> RNotificationTypeUserRoles { get; set; } = new List<RNotificationTypeUserRole>();
    public virtual ICollection<AppNotification> AppNotifications { get; set; } = new List<AppNotification>();
    public virtual ICollection<RUserNotification> RUserNotifications { get; set; } = new List<RUserNotification>();
    public virtual ICollection<RUserNotificationBlacklistSetting> RUserNotificationBlacklistSettings { get; set; } = new List<RUserNotificationBlacklistSetting>();
}

public sealed class CNotificationTypeConfiguration : IEntityTypeConfiguration<CNotificationType>
{
    public void Configure(EntityTypeBuilder<CNotificationType> entity)
    {
        entity.HasKey(e => e.NotificationTypeId)
            .HasName("pk_c_notification_type");

        entity.ToTable("c_notification_type");

        entity.HasIndex(e => e.EnumKey, "ix_c_notification_type_enum_key").IsUnique();

        entity.HasData(CNotificationTypeSeeder.Entities);
    }
}