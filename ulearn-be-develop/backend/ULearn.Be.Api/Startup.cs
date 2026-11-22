using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ULearn.Be.Api.Common.CustomMiddleware.Extensions;
using ULearn.Be.Api.Common.Security.Extensions;
using ULearn.Be.Api.Configuration;
using ULearn.Be.Common.Services.Infrustructure;
using ULearn.Be.Common.Services.Utilities.Infrastucture;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.Infrastructure;
using ULearn.Be.Repositories.Infrastructure;
using ULearn.Be.Services.HandlerServices.Infrastructure;
using ULearn.Be.Services.Services.Infrastructure;
using ULearn.Be.Shared.Services.Infrastructure;
using ULearn.Be.Websocket.Services.Hubs.Infrastucture;

namespace ULearn.Be.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("RepositoryDb");
        services.AddControllers()
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        services.AddSignalR().AddNewtonsoftJsonProtocol();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations(true, true);
            c.UseAllOfForInheritance();
        });
        services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseNpgsql(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                });
                options.EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment);
            }
        );
        services.AddRepositories();
        services.AddCommonServices();
        services.AddServices();
        services.AddSharedServices();
        services.AddHandlerServices();
        services.AddHubServices();
        services.AddUtilities();
        services.AddProviders(Configuration);
        services.AddUserRoleAuthorization();

        var envCfgProvider = services.AddEnvCfgProvider(Configuration);
        var authSettings = envCfgProvider.GetSettings<AuthSettings>(EnvSettingsKeyEnum.AuthSettings);
        var stripeSettings = envCfgProvider.GetSettings<StripeSettings>(EnvSettingsKeyEnum.StripeSettings);

        services.AddCookieAuthentication(authSettings.Domain);
        services.AddJwtAuthentication(authSettings.Secret);
        services.AddStripeConfiguration(stripeSettings.SecretKey);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(
            builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.SetIsOriginAllowed(_ => true);
                builder.AllowCredentials();
            }
        );
        app.ConfigureCustomExceptionMiddleware();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization()
            .UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = env.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None
            });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.AddHubs();
    }
}