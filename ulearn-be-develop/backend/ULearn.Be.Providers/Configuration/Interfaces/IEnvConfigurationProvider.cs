using ULearn.Be.Data.Enums;

namespace ULearn.Be.Providers.Configuration.Interfaces;

public interface IEnvConfigurationProvider
{
    Task<Dictionary<string, object>> GetConfigurationAllFe();
    void ReloadEnvSettings();
    T GetSettings<T>(EnvSettingsKeyEnum key, bool throwNotFoundException = true) where T : class;
    string EncryptPassword(string password);
    string DecryptPassword(string encryptedPassword);
}