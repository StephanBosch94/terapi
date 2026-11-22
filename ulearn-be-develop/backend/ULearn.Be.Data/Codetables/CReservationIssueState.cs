using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CReservationIssueState : CodetableBase
{
    [Column("reservation_issue_state_id")] public int ReservationIssueStateId { get; set; }
    [Column("icon_name")] public string IconName { get; set; }
    public ICollection<AppReservationIssue> AppReservationIssues { get; set; }
}

public sealed class CReservationIssueStateConfiguration : IEntityTypeConfiguration<CReservationIssueState>
{
    public void Configure(EntityTypeBuilder<CReservationIssueState> entity)
    {
        entity.HasKey(e => e.ReservationIssueStateId)
            .HasName("pk_c_reservation_issue_state");

        entity.ToTable("c_reservation_issue_state");

        entity.HasIndex(e => e.EnumKey, "ix_c_reservation_issue_state_enum_key").IsUnique();

        entity.HasData(CReservationIssueStateSeeder.Entities);
    }
}