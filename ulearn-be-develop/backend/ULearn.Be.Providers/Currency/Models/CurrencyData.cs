using Newtonsoft.Json;

namespace ULearn.Be.Providers.Currency.Models;

public class CurrencyData
{
    [JsonProperty("success")] public bool Success { get; set; }
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("base")] public string Base { get; set; }
    [JsonProperty("date")] public string Date { get; set; }

    [JsonProperty("rates")]
    public Dictionary<string, double> Rates { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}