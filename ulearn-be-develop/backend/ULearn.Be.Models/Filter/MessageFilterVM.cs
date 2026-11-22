using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class MessageFilterVM
{
    public string TextValue { get; set; }
    [DefaultSortColumn(1, true)] public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }
}