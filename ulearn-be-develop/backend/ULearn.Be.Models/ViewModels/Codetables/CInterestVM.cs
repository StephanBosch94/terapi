using Newtonsoft.Json;

namespace ULearn.Be.Models.ViewModels.Codetables;

public class CInterestVM : CodetableBaseVM
{
    public int InterestId { get; set; }
    public string IconName { get; set; }
    [JsonIgnore] public int UserId { get; set; }
}