using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppUserBehaviour
{
    [Column("user_behaviour_id")] public int UserBehaviourId { get; set; }
    [Column("connection_uuid")] public string ConnectionUuid { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("connected")] public DateTime Connected { get; set; }
    [Column("disconnected")] public DateTime? Disconnected { get; set; }
    [Column("url")] public string Url { get; set; }
    public AppUser User { get; set; }
}

public sealed class AppUserBehaviourTransactionConfiguration : IEntityTypeConfiguration<AppUserBehaviour>
{
    public void Configure(EntityTypeBuilder<AppUserBehaviour> entity)
    {
        entity.HasKey(e => e.UserBehaviourId)
            .HasName("pk_app_user_behaviour");

        entity.ToTable("app_user_behaviour");

        entity.HasIndex(e => e.UserId, "ix_app_user_behaviour_user_id");
        entity.HasIndex(e => e.ConnectionUuid, "ix_app_user_behaviour_connection_uuid").IsUnique();
        entity.HasIndex(e => new { e.Connected, e.Disconnected }, "ix_app_user_behaviour_connected_disconnected");

        entity.HasOne(e => e.User)
            .WithMany(e => e.AppUserBehaviours)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_app_user_behaviour_app_user_user_id");
    }
}