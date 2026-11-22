using Newtonsoft.Json;

namespace ULearn.Be.Providers.HubSpot.Models;

public class HubSpotInputPropertiesVM
{
    [JsonProperty("company")] public string Company { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
    [JsonProperty("firstname")] public string FirstName { get; set; }
    [JsonProperty("lastname")] public string LastName { get; set; }
    [JsonProperty("phone")] public string Phone { get; set; }
    [JsonProperty("website")] public string Website { get; set; }
    [JsonProperty("user_state")] public string State { get; set; }
    [JsonProperty("hourrate")] public double? HourRate { get; set; }
    [JsonProperty("role")] public string Role { get; set; }
    [JsonProperty("iana_timezone")] public string IanaTimezone { get; set; }
    [JsonProperty("ico")] public string Ico { get; set; }
    [JsonProperty("vat")] public string Vat { get; set; }
    [JsonProperty("is_vat_payer")] public bool? IsVatPayer { get; set; }
    [JsonProperty("business_address")] public string BusinessAddress { get; set; }
    [JsonProperty("subject")] public string Subject { get; set; }
}