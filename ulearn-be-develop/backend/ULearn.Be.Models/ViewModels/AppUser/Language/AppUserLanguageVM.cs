using Newtonsoft.Json;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppUser.Language;

public class AppUserLanguageVM
{
    [JsonIgnore] public int UserId { get; set; }
    public CLanguageVM Language { get; set; }
    public CLanguageLevelVM Level { get; set; }
}