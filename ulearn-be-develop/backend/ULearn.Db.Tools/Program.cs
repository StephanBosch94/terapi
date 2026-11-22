using Dawiso.App.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ULearn.Db.Tools;

internal class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        var serviceProvider = Startup.ConfigureServices(services);

        Log.Logger = CreateLogger(Startup.GetConfiguration());

        var app = serviceProvider.GetService<Application>();
        app.Run(args);
    }

    private static ILogger CreateLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}