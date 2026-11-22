using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data
{
    public partial class EnvConfigurationDataType
    {
        public EnvConfigurationDataType()
        {
            EnvConfigurations = new HashSet<EnvConfiguration>();
        }

        public int DataTypeId { get; set; }
        public string EnumKey { get; set; } = null!;
        public string NameKey { get; set; } = null!;
        public string DescriptionKey { get; set; } = null!;

        public virtual ICollection<EnvConfiguration> EnvConfigurations { get; set; }
    }
}

public sealed class EnvConfigurationDataTypeConfiguration : IEntityTypeConfiguration<EnvConfigurationDataType>
{
    public void Configure(EntityTypeBuilder<EnvConfigurationDataType> entity)
    {
        entity.HasKey(e => e.DataTypeId)
            .HasName("pk_env_configuration_data_type");

        entity.ToTable("env_configuration_data_type");

        entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");

        entity.Property(e => e.DescriptionKey)
            .HasMaxLength(100)
            .HasColumnName("description_key");

        entity.Property(e => e.EnumKey)
            .HasMaxLength(100)
            .HasColumnName("enum_key");

        entity.Property(e => e.NameKey)
            .HasMaxLength(100)
            .HasColumnName("name_key");

        entity.HasData(EnvConfigurationDataTypeSeeder.Entities);
    }
}