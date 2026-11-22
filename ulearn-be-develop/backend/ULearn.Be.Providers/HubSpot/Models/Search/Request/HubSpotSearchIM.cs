using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models.Search.Request;

public class HubSpotSearchIM
{
    [JsonProperty("filterGroups")] public List<HubSpotFilterGroup> FilterGroups { get; set; }
}