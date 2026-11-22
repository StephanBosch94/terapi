using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;

namespace ULearn.Be.Data
{
    public partial class AppRecurrentTimeslot
    {
        [Column("recurrent_timeslot_id")] public int RecurrentTimeslotId { get; set; }
        [Column("owner_id")] public int OwnerId { get; set; }
        [Column("local_iso_weekday")] public int LocalIsoWeekDay { get; set; }
        [Column("from_local_hour")] public int FromLocalHour { get; set; }
        [Column("from_local_minute")] public int FromLocalMinute { get; set; }
        [Column("duration_in_minutes")] public int DurationInMinutes { get; set; }
        [Column("iana_timezone")] public string IanaTimezone { get; set; }
        public virtual AppUser Owner { get; set; } = null!;
        public virtual ICollection<AppRecurrentTimeslotBlacklist> Blacklists { get; set; } = null!;
    }
}

public sealed class AppRecurrentTimeslotConfiguration : IEntityTypeConfiguration<AppRecurrentTimeslot>
{
    public void Configure(EntityTypeBuilder<AppRecurrentTimeslot> entity)
    {
        entity.HasKey(e => e.RecurrentTimeslotId)
            .HasName("pk_app_recurrent_timeslot");

        entity.HasIndex(e => e.OwnerId, "ix_app_recurrent_timeslot_owner_id");

        entity.ToTable("app_recurrent_timeslot");

        entity.HasOne(d => d.Owner)
            .WithMany(p => p.AppRecurrentTimeslots)
            .HasForeignKey(d => d.OwnerId)
            .HasConstraintName("fk_app_recurrent_timeslot_app_user_owner_id");
    }
}