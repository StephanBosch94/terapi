using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models.Search.Response;

public class HubSpotSearchPagedVM
{
    [JsonProperty("total")] public int Total { get; set; }
    [JsonProperty("results")] public List<HubSpotSearchResultVM> Results { get; set; }
}