using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public partial class AppUserRegistrationCode
{
    public int UserRegistrationCodeId { get; set; }
    public int UserId { get; set; }
    public string RegistrationCode { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Used { get; set; }
    public virtual AppUser AppUser { get; set; }
}

public sealed class AppUserRegistrationCodeConfiguration : IEntityTypeConfiguration<AppUserRegistrationCode>
{
    public void Configure(EntityTypeBuilder<AppUserRegistrationCode> entity)
    {
        entity.HasKey(e => e.UserRegistrationCodeId)
            .HasName("pk_app_user_registration_code");

        entity.ToTable("app_user_registration_code");

        entity.HasIndex(e => e.UserId, "ix_app_user_registration_code_user_id");

        entity.HasIndex(e => e.RegistrationCode, "ix_app_user_registration_registration_code");

        entity.Property(e => e.UserRegistrationCodeId).HasColumnName("user_registration_code_id");

        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.Property(e => e.RegistrationCode)
            .HasColumnName("registration_code")
            .HasMaxLength(100);

        entity.Property(e => e.Created).HasColumnName("created");

        entity.Property(e => e.Used).HasColumnName("used");

        entity.HasOne(e => e.AppUser)
            .WithMany(e => e.AppUserRegistrationCodes)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("fk_app_user_registration_code_app_user_user_id");
    }
}