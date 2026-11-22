using Newtonsoft.Json;

namespace ULearn.Be.Models.ViewModels.AppUser.Specialization;

public class AppUserSpecializationVM
{
    [JsonIgnore] public int UserId { get; set; }
    public int SubjectId { get; set; }
    public int SpecializationId { get; set; }
    public string NameKey { get; set; }
    public string EnumKey { get; set; }
    public string DescriptionKey { get; set; }
    public string IconName { get; set; }
}