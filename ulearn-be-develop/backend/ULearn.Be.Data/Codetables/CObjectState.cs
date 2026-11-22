using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CObjectState : CodetableBase
{
    [Column("object_state_id")] public int ObjectStateId { get; set; }
    public virtual ICollection<CSubject> CSubjects { get; set; }
}

public sealed class CObjectStateConfiguration : IEntityTypeConfiguration<CObjectState>
{
    public void Configure(EntityTypeBuilder<CObjectState> entity)
    {
        entity.HasKey(e => e.ObjectStateId)
            .HasName("object_state_id");

        entity.ToTable("c_object_state");

        entity.HasIndex(e => e.ObjectStateId, "ix_c_object_state_object_state_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_object_state_enum_key").IsUnique();

        entity.HasData(CObjectStateSeeder.Entities);
    }
}