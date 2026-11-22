using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ULearn.Be.Crypto.Helpers;
using ULearn.Be.Providers.Configuration;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.Crypto;

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
        var configuration = GetConfiguration();

        services.AddTransient<Application>();
        services.AddLogging(x => x.AddSerilog());

        /* Add MasterPassword Settings */
        services.Configure<MasterPasswordSettings>(configuration.GetSection("MasterPasswordSettings"));

        services.AddSingleton<ICryptographyService, CryptographyService>();
        services.AddScoped<EncryptionHelper, EncryptionHelper>();

        return services.BuildServiceProvider();
    }

    public static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
            .AddEnvironmentVariables();

        return builder.Build();
    }
}