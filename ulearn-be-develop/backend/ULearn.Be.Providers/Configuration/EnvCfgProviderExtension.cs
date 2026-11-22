using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Providers.Configuration.Context;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.Providers.Configuration;

public static class EnvCfgProviderExtension
{
    public static IEnvConfigurationProvider AddEnvCfgProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        EnvCfgRepositoryContextFactory testFactory = null
    )
    {
        var cryptoService = new CryptographyService();
        services.AddSingleton<ICryptographyService>(cryptoService);

        if (string.CompareOrdinal(
                configuration.GetSection("CustomWebApplicationFactoryWithInMemoryDb").Value,
                "True"
            ) == 0)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<DbContextOptions<EnvCfgRepositoryContext>>();
            testFactory = new EnvCfgRepositoryContextFactory(options);
        }

        /* If it is used InMemoryDB - UnitTests */
        if (testFactory != null)
        {
            var masterPassword = configuration.GetSection("TestMasterPassword").Value ?? "fakeTestPassword";
            IEnvConfigurationProvider testEnvConfigurationProvider = new EnvConfigurationProvider(
                testFactory,
                cryptoService,
                masterPassword
            );
            services.AddSingleton(testEnvConfigurationProvider);

            return testEnvConfigurationProvider;
        }

        /* Read configuration from DB */

        var optionsBuilder = new DbContextOptionsBuilder<EnvCfgRepositoryContext>();
        var connectionString = configuration.GetConnectionString("RepositoryDb");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new NotFoundException("Missing RepositoryDb connection string");
        }

        optionsBuilder.UseNpgsql(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
            })
            .EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment);
        var envCfgRepositoryContextFactory = new EnvCfgRepositoryContextFactory(optionsBuilder.Options);

        var masterPasswordSettings = configuration.GetSection("MasterPasswordSettings").Get<MasterPasswordSettings>();
        if (masterPasswordSettings == null)
        {
            throw new NotFoundException("Missing MasterPasswordSettings section in appsetting.json");
        }

        IEnvConfigurationProvider envCfgProvider = new EnvConfigurationProvider(
            envCfgRepositoryContextFactory,
            cryptoService,
            masterPasswordSettings.MasterPassword,
            configuration
        );
        services.AddSingleton(envCfgProvider);

        return envCfgProvider;
    }
}