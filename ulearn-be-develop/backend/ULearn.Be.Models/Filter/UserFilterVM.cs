using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class UserFilterVM
{
    [DefaultSortColumn(1)] public int? OrderNumber { get; set; }

    [DefaultSortColumn(2, true)]
    [IgnoreFromAutomaticFilter]
    public int? NumberOfLessons { get; set; }

    [DefaultSortColumn(3, true)]
    [IgnoreFromAutomaticFilter]
    public double? Rating { get; set; }

    [DefaultSortColumn(4, true)]
    [IgnoreFromAutomaticFilter]
    public int? NumberOfRatings { get; set; }

    [DefaultSortColumn(5)] public DateTime? Created { get; set; }
    public int? StateId { get; set; }
    public int? RoleId { get; set; }
    [IgnoreFromAutomaticFilter] public double? FromTrialPrice { get; set; }
    [IgnoreFromAutomaticFilter] public double? ToTrialPrice { get; set; }
    [IgnoreFromAutomaticFilter] public int? FromRating { get; set; }
    [IgnoreFromAutomaticFilter] public int? SubjectId { get; set; }
    [IgnoreFromAutomaticFilter] public int? CountryId { get; set; }
    [IgnoreFromAutomaticFilter] public IReadOnlyCollection<int> LanguageIds { get; set; }
    [IgnoreFromAutomaticFilter] public IReadOnlyCollection<int> SubjectLevelIds { get; set; }
    [IgnoreFromAutomaticFilter] public IReadOnlyCollection<int> SpecializationIds { get; set; }
    [IgnoreFromAutomaticFilter] public IReadOnlyCollection<int> InterestIds { get; set; }
}