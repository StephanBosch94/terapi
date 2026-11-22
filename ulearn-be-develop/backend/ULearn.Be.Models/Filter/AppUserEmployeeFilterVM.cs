using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public record AppUserEmployeeFilterVM
{
    [DefaultSortColumn(3)] public int? UserStateId { get; set; }
    [DefaultSortColumn(1)] public string FirstName { get; set; }
    [DefaultSortColumn(2)] public string LastName { get; set; }
}