using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CReservationIssueType : CodetableBase
{
    [Column("reservation_issue_type_id")] public int ReservationIssueTypeId { get; set; }

    [Column("has_description")] public bool HasDescription { get; set; }
    public ICollection<AppReservationIssue> AppReservationIssues { get; set; }
}

public sealed class CReservationIssueTypeConfiguration : IEntityTypeConfiguration<CReservationIssueType>
{
    public void Configure(EntityTypeBuilder<CReservationIssueType> entity)
    {
        entity.HasKey(e => e.ReservationIssueTypeId)
            .HasName("pk_c_reservation_issue_type");

        entity.ToTable("c_reservation_issue_type");

        entity.HasIndex(e => e.EnumKey, "uc_c_reservation_issue_type_enum_key").IsUnique();

        entity.HasData(CReservationIssueTypeSeeder.Entities);
    }
}