using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class AppTutorCardFilterVM
{
    [DefaultSortColumn(1)] public int? OrderNumber { get; set; }

    [DefaultSortColumn(2, true)] public int? NumberOfLessons { get; set; }

    [DefaultSortColumn(3, true)] public double? Rating { get; set; }

    [DefaultSortColumn(4, true)] public int? NumberOfRatings { get; set; }

    [DefaultSortColumn(5)] public DateTime? Created { get; set; }

    public int? SubjectId { get; set; }

    public int? CountryId { get; set; }
}