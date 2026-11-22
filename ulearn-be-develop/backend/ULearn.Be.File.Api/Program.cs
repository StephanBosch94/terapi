using System.Globalization;
using Serilog;

namespace ULearn.Be.File.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}