using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CWeeklyGoal : CodetableBase
{
    public int WeeklyGoalId { get; set; }
    public ICollection<AppUser> AppUsers { get; set; }
}

public sealed class CWeeklyGoalConfiguration : IEntityTypeConfiguration<CWeeklyGoal>
{
    public void Configure(EntityTypeBuilder<CWeeklyGoal> entity)
    {
        entity.HasKey(e => e.WeeklyGoalId)
            .HasName("pk_c_weekly_goal");

        entity.ToTable("c_weekly_goal");

        entity.Property(e => e.WeeklyGoalId).HasColumnName("weekly_goal_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_weekly_goal_enum_key").IsUnique();

        entity.HasData(CWeeklyGoalSeeder.Entities);
    }
}