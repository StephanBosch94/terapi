using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models.Search.Request;

public class HubSpotFilterGroup
{
    [JsonProperty("filters")] public List<HubSpotFilter> Filters { get; set; }
}