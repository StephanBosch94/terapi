using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.File.Data.Context;

public class FileRepositoryContext : DbContext
{
    private readonly IConfiguration _configuration;


    public FileRepositoryContext()
    {
    }

    public FileRepositoryContext(DbContextOptions<FileRepositoryContext> options) : base(options)
    {
    }

    public FileRepositoryContext(DbContextOptions<FileRepositoryContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<AppFile> AppFiles { get; set; }
    public virtual DbSet<RFileFormatCompressionType> RFileFormatCompressionTypes { get; set; }
    public virtual DbSet<CCompressionType> CCompressionTypes { get; set; }
    public virtual DbSet<CFileState> CFileStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = ConfigurationUtil.LoadConnectionString("RepositoryDb");
            optionsBuilder.UseNpgsql(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
            });
        }

        optionsBuilder.EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseSchemaConstants.Sfile);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileRepositoryContext).Assembly);
    }
}