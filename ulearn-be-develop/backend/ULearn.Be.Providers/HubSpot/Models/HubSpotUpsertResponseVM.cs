using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models;

public class HubSpotUpsertResponseVM
{
    [JsonProperty("id")] public string Id { get; set; }
}