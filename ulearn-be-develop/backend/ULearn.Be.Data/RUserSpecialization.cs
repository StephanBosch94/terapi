using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class RUserSpecialization
{
    public int UserSpecializationId { get; set; }
    public int UserId { get; set; }
    public int SpecializationId { get; set; }
    public virtual AppUser User { get; set; }
    public virtual CSpecialization Specialization { get; set; }
}

public sealed class RUserSpecializationConfiguration : IEntityTypeConfiguration<RUserSpecialization>
{
    public void Configure(EntityTypeBuilder<RUserSpecialization> entity)
    {
        entity.HasKey(e => e.UserSpecializationId)
            .HasName("pk_r_user_specialization");

        entity.ToTable("r_user_specialization");

        entity.Property(e => e.UserSpecializationId).HasColumnName("user_specialization_id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
        
        entity.HasIndex(e => e.UserId, "ix_r_user_specialization_user_id");
        
        entity.HasIndex(e => e.SpecializationId, "ix_r_user_specialization_specialization_id");

        entity.HasIndex(e => new { e.UserId, e.SpecializationId }, "uc_r_user_specialization_user_id_specialization")
            .IsUnique();
        
        entity.HasOne(e => e.User)
            .WithMany(e => e.RUserSpecializations)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_r_user_specialization_app_user_user_id");
        
        entity.HasOne(e => e.Specialization)
            .WithMany(e => e.RUserSpecializations)
            .HasForeignKey(e => e.SpecializationId)
            .HasConstraintName("fk_r_user_specialization_c_specialization_specialization_id");
    }
}