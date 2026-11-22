using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;

namespace ULearn.Be.Providers.Configuration.Context;

public class EnvCfgRepositoryContext : DbContext
{
    private readonly IConfiguration _configuration;

    public EnvCfgRepositoryContext()
    {
    }

    public EnvCfgRepositoryContext(DbContextOptions<EnvCfgRepositoryContext> options) : base(options)
    {
    }

    public EnvCfgRepositoryContext(DbContextOptions<EnvCfgRepositoryContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<EnvConfiguration> EnvConfiguration { get; set; }
    public virtual DbSet<EnvConfigurationDataType> EnvConfigurationDataType { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseSchemaConstants.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);

        /* Ignore all other types which are not required by EnvCfgProvider */
        var ignoreTypes = new List<Type>();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType != typeof(EnvConfiguration) &&
                entityType.ClrType != typeof(EnvConfigurationDataType))
            {
                ignoreTypes.Add(entityType.ClrType);
            }
        }

        ignoreTypes.ForEach(type => modelBuilder.Ignore(type));
    }
}