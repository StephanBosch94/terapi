using Newtonsoft.Json;

namespace Yaspeak.Monitoring.Status.Models;

public class SlackMessageVM
{
    [JsonProperty("text")] public string Text { get; set; }
}