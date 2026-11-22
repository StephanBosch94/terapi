using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppRecurrentTimeslotBlacklist
{
    public int RecurrentTimeslotBlacklistId { get; set; }
    public int RecurrentTimeslotId { get; set; }
    public int LocalYear { get; set; }
    public int LocalMonth { get; set; }
    public int LocalDay { get; set; }
    public virtual AppRecurrentTimeslot AppRecurrentTimeslot { get; set; } = null!;
}

public sealed class AppRecurrentTimeslotBlacklistConfiguration : IEntityTypeConfiguration<AppRecurrentTimeslotBlacklist>
{
    public void Configure(EntityTypeBuilder<AppRecurrentTimeslotBlacklist> entity)
    {
        entity.HasKey(e => e.RecurrentTimeslotBlacklistId);

        entity.ToTable("app_recurrent_timeslot_blacklist");

        entity.HasIndex(e => e.RecurrentTimeslotId,
            "ix_app_recurrent_timeslot_blacklist_user_recurrent_event_id");

        entity.Property(e => e.RecurrentTimeslotBlacklistId)
            .HasColumnName("recurrent_timeslot_blacklist_id");
        entity.Property(e => e.RecurrentTimeslotId).HasColumnName("recurrent_timeslot_id");
        entity.Property(e => e.LocalYear).HasColumnName("local_year");
        entity.Property(e => e.LocalMonth).HasColumnName("local_month");
        entity.Property(e => e.LocalDay).HasColumnName("local_day");

        entity.HasOne(d => d.AppRecurrentTimeslot)
            .WithMany(p => p.Blacklists)
            .HasForeignKey(d => d.RecurrentTimeslotId)
            .HasConstraintName(
                "fk_app_recurrent_timeslot_blacklist_app_recurrent_timeslot_recurrent_timeslot_id");
    }
}