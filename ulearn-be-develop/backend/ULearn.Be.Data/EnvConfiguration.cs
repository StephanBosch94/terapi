using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data
{
    public partial class EnvConfiguration
    {
        public int ConfigurationId { get; set; }
        public int DataTypeId { get; set; }
        public string ConfigurationKey { get; set; } = null!;
        public string ConfigurationValue { get; set; } = null!;
        public bool AdminConfigurable { get; set; }
        public bool BackendOnly { get; set; }
        public string ValueOrigin { get; set; } = null!;

        public virtual EnvConfigurationDataType DataType { get; set; } = null!;
    }
}

public sealed class EnvConfigurationConfiguration : IEntityTypeConfiguration<EnvConfiguration>
{
    public void Configure(EntityTypeBuilder<EnvConfiguration> entity)
    {
        entity.HasKey(e => e.ConfigurationId)
            .HasName("pk_env_configuration");

        entity.ToTable("env_configuration");

        entity.HasIndex(e => e.DataTypeId, "ix_env_configuration_data_type_data_type_id");

        entity.Property(e => e.ConfigurationId).HasColumnName("configuration_id");

        entity.Property(e => e.AdminConfigurable).HasColumnName("admin_configurable");

        entity.Property(e => e.BackendOnly).HasColumnName("backend_only");

        entity.Property(e => e.ConfigurationKey)
            .HasMaxLength(256)
            .HasColumnName("configuration_key");

        entity.Property(e => e.ConfigurationValue)
            .HasColumnName("configuration_value");

        entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");

        entity.HasIndex(e => e.BackendOnly, "ix_env_configuration_backend_only");
        
        entity.Property(e => e.ValueOrigin)
            .HasMaxLength(256)
            .HasColumnName("value_origin");

        entity.HasOne(d => d.DataType)
            .WithMany(p => p.EnvConfigurations)
            .HasForeignKey(d => d.DataTypeId)
            .HasConstraintName("fk_env_configuration_env_configuration_data_type_data_type_id");

        entity.HasData(EnvConfigurationSeeder.Entities);
    }
}