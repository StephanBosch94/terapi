using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class HubSpotSettings
{
    [EncryptedByMasterPassword] public string ApiKey { get; set; }
}