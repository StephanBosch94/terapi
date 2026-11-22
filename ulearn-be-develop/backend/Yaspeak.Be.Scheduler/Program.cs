using System.Globalization;
using Serilog;
using Yaspeak.Be.Scheduler;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => { Startup.ConfigureServices(context.Configuration, services); })
    .UseSerilog((hostingContext, _, loggerConfiguration) =>
        loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration)
            .Enrich.FromLogContext()
    ).Build();

await host.RunAsync();