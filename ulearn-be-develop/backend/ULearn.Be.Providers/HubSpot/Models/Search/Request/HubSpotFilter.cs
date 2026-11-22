using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models.Search.Request;

public class HubSpotFilter
{
    [JsonProperty("propertyName")] public string PropertyName { get; set; }
    [JsonProperty("operator")] public string Operator { get; set; }
    [JsonProperty("value")] public string Value { get; set; }
}