using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ULearn.Be.Api.Common.CustomMiddleware.Extensions;
using ULearn.Be.Api.Common.Security.Extensions;
using ULearn.Be.Common.Services.Utilities.Infrastucture;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.File.Data.Context;
using ULearn.Be.File.Service.Services.Extensions;
using ULearn.Be.Providers.Configuration;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.File.Api;

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
        services.AddControllers();
        var connectionString = Configuration.GetConnectionString("RepositoryDb");
        services.AddDbContext<FileRepositoryContext>(options =>
            options.UseNpgsql(connectionString,
                builder =>
                {
                    builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                }).EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment)
        );
        /* Add MasterPassword Settings */
        services.Configure<MasterPasswordSettings>(Configuration.GetSection("MasterPasswordSettings"));

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.IncreaseMaxRequestLength();

        /* Add Swagger settings */
        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                    { Title = "ULearn", Version = "v1", Description = "ULearn" }
                );
            }
        );

        var envCfgProvider = services.AddEnvCfgProvider(Configuration);

        /* Add AddJwtAuthentication */
        var authSettings =
            envCfgProvider.GetSettings<AuthSettings>(EnvSettingsKeyEnum.AuthSettings);
        services.AddJwtAuthentication(authSettings.Secret);
        services.AddUserRoleAuthorization();

        /* Add FileService */
        var fileSettings =
            envCfgProvider.GetSettings<DiskSettings>(EnvSettingsKeyEnum.FileServiceDiskSettings);
        var isDisk = !string.IsNullOrWhiteSpace(fileSettings.FileStoragePath);
        services.AddFileServices(isDisk);
        services.AddCommonServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MMP"); });
        }

        app.UseRouting();
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
        app.UseAuthorization();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}