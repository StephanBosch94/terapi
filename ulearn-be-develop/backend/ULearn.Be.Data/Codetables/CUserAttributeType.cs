using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CUserAttributeType : CodetableBase
    {
        public CUserAttributeType()
        {
            RUserAttributes = new HashSet<RUserAttribute>();
        }

        public int UserAttributeTypeId { get; set; }
        public virtual ICollection<RUserAttribute> RUserAttributes { get; set; }
    }
}

public sealed class CUserAttributeTypeConfiguration : IEntityTypeConfiguration<CUserAttributeType>
{
    public void Configure(EntityTypeBuilder<CUserAttributeType> entity)
    {
        entity.HasKey(e => e.UserAttributeTypeId)
            .HasName("pk_c_user_attribute");

        entity.ToTable("c_user_attribute_type");

        entity.Property(e => e.UserAttributeTypeId).HasColumnName("user_attribute_type_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_attribute_type_enum_key").IsUnique();

        entity.HasData(CUserAttributeTypeSeeder.Entities);
    }
}