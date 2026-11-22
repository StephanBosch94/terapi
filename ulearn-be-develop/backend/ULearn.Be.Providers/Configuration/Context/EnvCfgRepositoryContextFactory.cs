using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ULearn.Be.Providers.Configuration.Interfaces;

namespace ULearn.Be.Providers.Configuration.Context;

public class EnvCfgRepositoryContextFactory : IEnvCfgRepositoryContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly DbContextOptions<EnvCfgRepositoryContext> _options;

    public EnvCfgRepositoryContextFactory(
        DbContextOptions<EnvCfgRepositoryContext> options,
        IConfiguration configuration = null
    )
    {
        _options = options;
        _configuration = configuration;
    }

    public EnvCfgRepositoryContext Get()
    {
        return new EnvCfgRepositoryContext(_options, _configuration);
    }
}