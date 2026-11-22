using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CAuthenticationMethod : CodetableBase
{
    public CAuthenticationMethod()
    {
        RegistrationMethodAppUsers = new HashSet<AppUser>();
    }

    [EnumKey("authentication_method_id")] public int AuthenticationMethodId { get; set; }
    public virtual ICollection<AppUser> RegistrationMethodAppUsers { get; set; }
}

public sealed class CAuthenticationMethodConfiguration : IEntityTypeConfiguration<CAuthenticationMethod>
{
    public void Configure(EntityTypeBuilder<CAuthenticationMethod> entity)
    {
        entity.HasKey(e => e.AuthenticationMethodId)
            .HasName("pk_c_authentication_method");

        entity.ToTable("c_authentication_method");

        entity.HasIndex(e => e.EnumKey, "ix_c_authentication_method_enum_key").IsUnique();

        entity.HasData(CAuthenticationMethodSeeder.Entities);
    }
}