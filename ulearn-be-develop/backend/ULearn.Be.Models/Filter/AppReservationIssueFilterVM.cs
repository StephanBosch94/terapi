using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class AppReservationIssueFilterVM
{
    public int? IssueTypeId { get; set; }
    [DefaultSortColumn(2)] public int? IssueStateId { get; set; }
    public string Description { get; set; }
    [DefaultSortColumn(1)] public DateTime? Created { get; set; }
}