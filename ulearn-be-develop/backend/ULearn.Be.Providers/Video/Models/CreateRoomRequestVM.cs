using Newtonsoft.Json;

namespace ULearn.Be.Providers.Video.Models;

public class CreateRoomRequestVM
{
    [JsonProperty("autoCloseConfig")] public AutoCloseConfigVM AutoCloseConfig { get; set; }
}

public class AutoCloseConfigVM
{
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("duration")] public int Duration { get; set; }
}