using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class RUserAttribute
    {
        public int UserAttributeId { get; set; }
        public int UserId { get; set; }
        public int UserAttributeTypeId { get; set; }
        public string Value { get; set; }

        public virtual AppUser User { get; set; } = null!;
        public virtual CUserAttributeType UserAttributeType { get; set; } = null!;
    }
}

public sealed class RUserAttributeConfiguration : IEntityTypeConfiguration<RUserAttribute>
{
    public void Configure(EntityTypeBuilder<RUserAttribute> entity)
    {
        entity.HasKey(e => e.UserAttributeId)
            .HasName("pk_r_user_attribute");

        entity.ToTable("r_user_attribute");

        entity.HasIndex(e => e.UserAttributeTypeId, "ix_r_user_attribute_user_attribute_type_id");

        entity.HasIndex(e => e.UserId, "ix_r_user_attribute_user_id");

        entity.Property(e => e.UserAttributeId).HasColumnName("user_attribute_id");

        entity.Property(e => e.UserAttributeTypeId).HasColumnName("user_attribute_type_id");

        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.Property(e => e.Value).HasColumnName("value");

        entity.HasIndex(e => new { e.UserId, e.UserAttributeTypeId }, "uc_r_user_attribute_user_id_user_attribute_type_id")
            .IsUnique();
        
        entity.HasOne(d => d.UserAttributeType)
            .WithMany(p => p.RUserAttributes)
            .HasForeignKey(d => d.UserAttributeTypeId)
            .HasConstraintName("fk_r_user_attribute_c_user_attribute_type");

        entity.HasOne(d => d.User)
            .WithMany(p => p.RUserAttributes)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_attribute_app_user_user_id");
    }
}