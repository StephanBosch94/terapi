using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppReservationIssue
{
    [Column("reservation_issue_id")] public int ReservationIssueId { get; set; }
    [Column("reservation_issue_type_id")] public int ReservationIssueTypeId { get; set; }
    [Column("reservation_id")] public int ReservationId { get; set; }
    [Column("reservation_issue_state_id")] public int ReservationIssueStateId { get; set; }
    [Column("description")] public string Description { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("resolved")] public DateTime? Resolved { get; set; }
    [Column("creator_id")] public int CreatorId { get; set; }
    [Column("resolver_id")] public int? ResolverId { get; set; }
    [Column("decision_text")] public string DecisionText { get; set; }
    public CReservationIssueState ReservationIssueState { get; set; }
    public CReservationIssueType ReservationIssueType { get; set; }
    public AppReservation AppReservation { get; set; }
    public AppUser Creator { get; set; }
    public AppUser Resolver { get; set; }
    public ICollection<AppTransaction> AppTransactions { get; set; }
}

public sealed class AppReservationIssueConfiguration : IEntityTypeConfiguration<AppReservationIssue>
{
    public void Configure(EntityTypeBuilder<AppReservationIssue> entity)
    {
        entity.HasKey(e => e.ReservationIssueId)
            .HasName("pk_app_reservation_issue");

        entity.ToTable("app_reservation_issue");

        entity.HasIndex(e => e.ReservationIssueTypeId, "ix_app_reservation_reservation_issue_type_id");
        entity.HasIndex(e => e.ReservationIssueStateId, "ix_app_reservation_reservation_issue_state_id");
        entity.HasIndex(e => e.CreatorId, "ix_app_reservation_creator_id");
        entity.HasIndex(e => e.ResolverId, "ix_app_reservation_updater_id");

        entity.HasOne(e => e.ReservationIssueType)
            .WithMany(e => e.AppReservationIssues)
            .HasForeignKey(e => e.ReservationIssueTypeId)
            .HasConstraintName("fk_app_reservation_issue_c_reservation_issue_type_reservation_issue_type_id");

        entity.HasOne(e => e.Creator)
            .WithMany(e => e.CreatedAppReservationIssues)
            .HasForeignKey(e => e.CreatorId)
            .HasConstraintName("fk_app_reservation_issue_app_user_creator_id");

        entity.HasOne(e => e.Resolver)
            .WithMany(e => e.ResolvedAppReservationIssues)
            .HasForeignKey(e => e.ResolverId)
            .HasConstraintName("fk_app_reservation_issue_app_user_resolver_id");

        entity.HasOne(e => e.AppReservation)
            .WithMany(e => e.AppReservationIssues)
            .HasForeignKey(e => e.ReservationId)
            .HasConstraintName("fk_app_reservation_issue_app_reservation_reservation_id");

        entity.HasOne(e => e.ReservationIssueState)
            .WithMany(e => e.AppReservationIssues)
            .HasForeignKey(e => e.ReservationIssueStateId)
            .HasConstraintName("fk_app_reservation_issue_c_reservation_issue_state_reservation_issue_state_id");
    }
}