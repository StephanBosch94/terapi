using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class FacebookApiSettings
{
    public string ClientId { get; set; }
    [EncryptedByMasterPassword] public string ClientSecret { get; set; }
    public string TutorRedirectUrl { get; set; }
    public string StudentRedirectUrl { get; set; }
    public string OauthUrl { get; set; }
    public List<string> Scopes { get; set; }
}