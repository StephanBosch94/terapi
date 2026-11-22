using Newtonsoft.Json;

namespace ULearn.Be.Providers.Configuration.Models.External;

public class SeznamAuthResultVM
{
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; }

    [JsonProperty(PropertyName = "account_name")]
    public string AccountName { get; set; }

    [JsonProperty(PropertyName = "expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    [JsonProperty(PropertyName = "oauth_user_id")]
    public string OauthUserId { get; set; }

    [JsonProperty(PropertyName = "refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty(PropertyName = "scopes")]
    public List<string> Scopes { get; set; }

    [JsonProperty(PropertyName = "status")]
    public int Status { get; set; }

    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; set; }
}