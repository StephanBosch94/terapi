using Newtonsoft.Json;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppUser.Attribute;

public class UserAttributeVM
{
    [JsonIgnore] public int UserId { get; set; }
    public CUserAttributeTypeVM AttributeType { get; set; }
    public string Value { get; set; }
}