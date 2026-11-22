using Newtonsoft.Json;

namespace ULearn.Be.Providers.Configuration.Models.External;

public class SeznamUserDataVM
{
    [JsonProperty(PropertyName = "advert_user_id")]
    public string AdvertUserId { get; set; }

    [JsonProperty(PropertyName = "avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty(PropertyName = "contact_phone")]
    public string ContactPhone { get; set; }

    [JsonProperty(PropertyName = "domain")]
    public string Domain { get; set; }

    [JsonProperty(PropertyName = "firstname")]
    public string Firstname { get; set; }

    [JsonProperty(PropertyName = "lastname")]
    public string Lastname { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    [JsonProperty(PropertyName = "oauth_user_id")]
    public string OauthUserId { get; set; }

    [JsonProperty(PropertyName = "status")]
    public int Status { get; set; }

    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; }
}