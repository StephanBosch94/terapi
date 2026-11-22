using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppUserProfileBehaviour
{
    [Column("user_profile_behaviour_id")] public int UserProfileBehaviourId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("visited_user_id")] public int VisitedUserId { get; set; }
    [Column("created")] public DateTime Created { get; set; }

    public AppUser User { get; set; }
    public AppUser VisitedUser { get; set; }
}

public sealed class AppUserProfileBehaviourConfiguration : IEntityTypeConfiguration<AppUserProfileBehaviour>
{
    public void Configure(EntityTypeBuilder<AppUserProfileBehaviour> entity)
    {
        entity.HasKey(e => e.UserProfileBehaviourId)
            .HasName("pk_app_user_profile_behaviour");

        entity.ToTable("app_user_profile_behaviour");

        entity.HasIndex(e => e.UserId, "ix_app_user_profile_behaviour_user_id");

        entity.HasIndex(e => e.VisitedUserId, "ix_app_user_profile_behaviour_visited_user_id");
        
        entity.HasOne(d => d.User)
            .WithMany(p => p.UserAppUserProfileBehaviours)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_app_user_profile_behaviour_app_user_user_id");

        entity.HasOne(d => d.VisitedUser)
            .WithMany(p => p.VisitedUserAppUserProfileBehaviours)
            .HasForeignKey(d => d.VisitedUserId)
            .HasConstraintName("fk_app_user_profile_behaviour_app_user_visited_user_id");
    }
}