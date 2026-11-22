using Newtonsoft.Json;

namespace ULearn.Be.Providers.Configuration.Models.External;

public class FacebookAuthResultVM
{
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; }

    [JsonProperty(PropertyName = "expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; set; }
}