using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class RUserInterest
{
    [Column("user_interest_id")] public int UserInterestId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("interest_id")] public int InterestId { get; set; }
    public AppUser User { get; set; }
    public CInterest CInterest { get; set; }
}

public sealed class RUserInterestConfiguration : IEntityTypeConfiguration<RUserInterest>
{
    public void Configure(EntityTypeBuilder<RUserInterest> entity)
    {
        entity.HasKey(e => e.UserInterestId)
            .HasName("pk_r_user_interest");

        entity.ToTable("r_user_interest");

        entity.HasIndex(e => e.UserId, "ix_r_user_interest_user_id");

        entity.HasIndex(e => e.InterestId, "ix_r_user_interest_interest_id");

        entity.HasIndex(e => new { e.UserId, e.InterestId }, "uc_r_user_interest_user_id_interest_id")
            .IsUnique();

        entity.HasOne(e => e.User)
            .WithMany(e => e.RUserInterests)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_r_user_interest_app_user_user_id");

        entity.HasOne(e => e.CInterest)
            .WithMany(e => e.RUserInterests)
            .HasForeignKey(e => e.InterestId)
            .HasConstraintName("fk_r_user_interest_c_interest_interest_id");
    }
}