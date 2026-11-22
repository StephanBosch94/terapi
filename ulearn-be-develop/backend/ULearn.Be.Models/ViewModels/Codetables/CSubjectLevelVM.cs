using Newtonsoft.Json;

namespace ULearn.Be.Models.ViewModels.Codetables;

public class CSubjectLevelVM
{
    [JsonIgnore] public int UserId { get; set; }
    public string EnumKey { get; set; }
    public string TutorNameKey { get; set; }
    public string StudentNameKey { get; set; }
    public string DescriptionKey { get; set; }
    public string IconName { get; set; }
    public string InvertedIconName { get; set; }
    public int SubjectLevelId { get; set; }
}