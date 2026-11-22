using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CCountry : CodetableBase
{
    public CCountry()
    {
        AppUsers = new HashSet<AppUser>();
    }

    [Column("country_id")] public int CountryId { get; set; }
    [Column("country_code")] public string CountryCode { get; set; }
    public virtual ICollection<AppUser> AppUsers { get; set; }
}

public sealed class CCountryConfiguration : IEntityTypeConfiguration<CCountry>
{
    public void Configure(EntityTypeBuilder<CCountry> entity)
    {
        entity.HasKey(e => e.CountryId)
            .HasName("pk_c_country");

        entity.ToTable("c_country");
        
        entity.HasIndex(e => e.EnumKey, "ix_c_country_enum_key").IsUnique();

        entity.HasData(CCountrySeeder.Entities);
    }
}