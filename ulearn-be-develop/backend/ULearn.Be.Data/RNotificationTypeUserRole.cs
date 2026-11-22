using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data;

public class RNotificationTypeUserRole
{
    [Column("notification_type_user_role_id")] public int NotificationTypeRoleId { get; set; }
    [Column("notification_type_id")] public int NotificationTypeId { get; set; }
    [Column("user_role_id")] public int UserRoleId { get; set; }

    public CNotificationType CNotificationType { get; set; }
    public CUserRole CUserRole { get; set; }
}

public sealed class RNotificationTypeRoleConfiguration : IEntityTypeConfiguration<RNotificationTypeUserRole>
{
    public void Configure(EntityTypeBuilder<RNotificationTypeUserRole> entity)
    {
        entity.HasKey(e => e.NotificationTypeRoleId)
            .HasName("pk_r_notification_type_user_role");

        entity.ToTable("r_notification_type_user_role");

        entity.HasIndex(e => e.NotificationTypeId, "ix_r_notification_type_user_role_notification_type_id");

        entity.HasIndex(e => e.UserRoleId, "ix_r_notification_type_user_role_user_role_id");

        entity.HasOne(d => d.CNotificationType)
            .WithMany(p => p.RNotificationTypeUserRoles)
            .HasForeignKey(d => d.NotificationTypeId)
            .HasConstraintName("fk_r_notification_type_user_role_c_notification_type_notification_type_id");

        entity.HasOne(d => d.CUserRole)
            .WithMany(p => p.RNotificationTypeUserRoles)
            .HasForeignKey(d => d.UserRoleId)
            .HasConstraintName("fk_r_notification_type_user_role_c_user_role_user_role_id");

        entity.HasData(RNotificationTypeUserRoleSeeder.Entities);
    }
}