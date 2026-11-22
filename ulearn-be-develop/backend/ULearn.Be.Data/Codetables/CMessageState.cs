using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CMessageState: CodetableBase
{
    public int MessageStateId { get; set; }
    public virtual ICollection<AppMessage> AppMessages { get; set; }
}

public sealed class CMessageStateConfiguration : IEntityTypeConfiguration<CMessageState>
{
    public void Configure(EntityTypeBuilder<CMessageState> entity)
    {
        entity.HasKey(e => e.MessageStateId)
            .HasName("pk_c_message_state");

        entity.ToTable("c_message_state");

        entity.Property(e => e.MessageStateId).HasColumnName("message_state_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_message_state_enum_key").IsUnique();

        entity.HasData(CMessageStateSeeder.Entities);
    }
}