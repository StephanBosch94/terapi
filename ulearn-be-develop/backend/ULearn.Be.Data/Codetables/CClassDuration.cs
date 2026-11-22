using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CClassDuration : CodetableBase
{
    [Column("class_duration_id")] public int ClassDurationId { get; set; }
    [Column("duration_with_margin")] public int DurationWithMargin { get; set; }
    [Column("expected_duration")] public int ExpectedDuration { get; set; }
    public ICollection<AppReservation> AppReservations { get; set; }
    public ICollection<AppReservationPackage> AppReservationPackages { get; set; }
}

public sealed class CClassDurationConfiguration : IEntityTypeConfiguration<CClassDuration>
{
    public void Configure(EntityTypeBuilder<CClassDuration> entity)
    {
        entity.HasKey(e => e.ClassDurationId)
            .HasName("pk_c_class_duration");

        entity.ToTable("c_class_duration");

        entity.HasIndex(e => e.EnumKey, "ix_c_class_duration_enum_key").IsUnique();

        entity.HasData(CClassDurationSeeder.Entities);
    }
}