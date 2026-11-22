using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ULearn.Db.Tools;
using ULearn.Db.Tools.Handlers;

namespace Dawiso.App.Db;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public static IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<Application>();
        services.AddTransient<CreateDatabaseHandler>();
        services.AddTransient<DropDatabaseHandler>();
        services.AddTransient<ResetDatabaseHandler>();
        services.AddTransient<RunSqlScriptsHandler>();
        services.AddTransient<TranslationUploaderHandler>();
        services.AddTransient<UploadProfilePhotosHandler>();
        services.AddTransient<WordGeneratorHandler>();
        services.AddTransient<DomainNameCheckHandler>();
        services.AddTransient<DeleteAccountHandler>();
        services.AddTransient<TsvTranslationUploadHandler>();
        services.AddTransient<UpdateTransactionTemplateHandler>();
        services.AddLogging(x => x.AddSerilog());

        return services.BuildServiceProvider();
    }

    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
            .AddEnvironmentVariables()
            .Build();
    }
}