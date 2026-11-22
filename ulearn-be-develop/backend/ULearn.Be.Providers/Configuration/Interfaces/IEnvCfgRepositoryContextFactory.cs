using ULearn.Be.Providers.Configuration.Context;

namespace ULearn.Be.Providers.Configuration.Interfaces;

public interface IEnvCfgRepositoryContextFactory
{
    EnvCfgRepositoryContext Get();
}