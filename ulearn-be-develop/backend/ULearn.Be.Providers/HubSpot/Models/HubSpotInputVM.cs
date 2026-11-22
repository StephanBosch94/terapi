using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models;

public class HubSpotInputVM
{
    [JsonProperty("properties")] public HubSpotInputPropertiesVM Properties { get; set; }
}
