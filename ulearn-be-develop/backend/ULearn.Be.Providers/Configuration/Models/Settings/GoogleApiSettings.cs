using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class GoogleApiSettings
{
    public string ClientId { get; set; }
    [EncryptedByMasterPassword] public string ClientSecret { get; set; }
    public string OauthUrl { get; set; }
    public List<string> TutorScopes { get; set; }
    public List<string> StudentScopes { get; set; }
}