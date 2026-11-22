using Microsoft.EntityFrameworkCore;
using Quartz;
using ULearn.Be.Api.Common.Security.Extensions;
using ULearn.Be.Common.Services.Infrustructure;
using ULearn.Be.Common.Services.Utilities.Infrastucture;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Context;
using ULearn.Be.Providers.Configuration;
using ULearn.Be.Providers.Infrastructure;
using ULearn.Be.Repositories.Infrastructure;
using ULearn.Be.Shared.Services.Infrastructure;
using ULearn.Be.Websocket.Services.Hubs.Infrastucture;
using Yaspeak.Be.Scheduler.Jobs;

namespace Yaspeak.Be.Scheduler;

public static class Startup
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("RepositoryDb");

        services.AddDbContext<RepositoryContext>(options =>
            options.UseNpgsql(connectionString,
                builder =>
                {
                    builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                }).EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment)
        );
        services.AddRepositories();
        services.AddSharedServices();
        services.AddHubServices();
        services.AddCommonServices();
        services.AddUtilities();
        services.AddProviders(configuration);
        services.AddUserRoleAuthorization();
        services.AddHostedService<Worker>();
        services.AddJobs();
        services.AddQuartz(c => { c.UseMicrosoftDependencyInjectionJobFactory(); });
        services.AddEnvCfgProvider(configuration);
        services.AddSignalR().AddNewtonsoftJsonProtocol();
    }
}