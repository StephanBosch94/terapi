using Newtonsoft.Json;

namespace ULearn.Be.Models.ViewModels.Email;

public class EmailUserVM
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
}