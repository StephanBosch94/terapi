using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models.Search.Response;

public class HubSpotSearchResultVM
{
    [JsonProperty("id")] public string Id { get; set; }
}