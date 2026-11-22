using Newtonsoft.Json;

namespace ULearn.Be.Providers.Video.Models;

public class CreateRoomResponseVM
{
    [JsonProperty("roomId")] public string RoomKey { get; set; }
}