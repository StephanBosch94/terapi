using Newtonsoft.Json;

namespace ULearn.Be.Providers.Configuration.Models.External;

public class FacebookUserDataVM
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
    [JsonProperty("birthday")] public string Birthday { get; set; }
    [JsonProperty("first_name")] public string FirstName { get; set; }
    [JsonProperty("last_name")] public string LastName { get; set; }
    [JsonProperty("picture")] public FacebookPicruteVM Picture { get; set; }
}

public class FacebookPicruteVM
{
    [JsonProperty("data")] public FacebookPicruteDataVM Data { get; set; }
}

public class FacebookPicruteDataVM
{
    [JsonProperty("url")] public string Url { get; set; }
}