using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppTimeslot
{
    [Column("timeslot_id")] public int TimeslotId { get; set; }
    [Column("from_time")] public DateTime FromTime { get; set; }
    [Column("owner_id")] public int OwnerId { get; set; }
    [Column("from_local_year")] public int FromLocalYear { get; set; }
    [Column("from_local_month")] public int FromLocalMonth { get; set; }
    [Column("from_local_day")] public int FromLocalDay { get; set; }
    [Column("from_local_hour")] public int FromLocalHour { get; set; }
    [Column("from_local_minute")] public int FromLocalMinute { get; set; }
    [Column("duration_in_minutes")] public int DurationInMinutes { get; set; }
    [Column("iana_timezone")] public string IanaTimezone { get; set; }
    public virtual AppUser Owner { get; set; } = null!;
}

public sealed class AppTimeslotConfiguration : IEntityTypeConfiguration<AppTimeslot>
{
    public void Configure(EntityTypeBuilder<AppTimeslot> entity)
    {
        entity.HasKey(e => e.TimeslotId);

        entity.ToTable("app_timeslot");

        entity.HasIndex(e => e.OwnerId, "ix_app_timeslot_owner_id");
        
        entity.HasOne(d => d.Owner)
            .WithMany(p => p.AppTimeslots)
            .HasForeignKey(d => d.OwnerId)
            .HasConstraintName("fk_app_user_event_app_user_owner_id");
    }
}