using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class AzureBlobSettings
{
    public string ContainerName { get; set; }
    public string Connection { get; set; }

    [EncryptedByMasterPassword] public string Password { get; set; }

    public string ConnectionString()
    {
        return Connection.Replace(EnvConfigurationProvider.PASSWORD_REPLACE_TOKEN, Password);
    }
}
