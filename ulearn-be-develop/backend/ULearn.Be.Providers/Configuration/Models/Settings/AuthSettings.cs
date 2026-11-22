using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class AuthSettings
{
    [EncryptedByMasterPassword] public string Secret { get; set; }
    public long ExpirationMs { get; set; }
    public string Issuer { get; set; }
    public string Domain { get; set; }
}