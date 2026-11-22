using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class EmailSettings
{
    [EncryptedByMasterPassword] public string ApiKey { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
}