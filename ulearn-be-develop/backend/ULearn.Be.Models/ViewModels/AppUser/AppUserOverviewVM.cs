using Newtonsoft.Json;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppUser.AppUserStatistics;
using ULearn.Be.Models.ViewModels.AppUser.Attribute;
using ULearn.Be.Models.ViewModels.AppUser.Language;
using ULearn.Be.Models.ViewModels.AppUser.Specialization;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.ViewModels.AppUser;

public class AppUserOverviewVM : UserFilterVM
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public new DateTime Created { get; set; }
    public new int StateId { get; set; }
    public new int RoleId { get; set; }
    public double? HourRate { get; set; }
    public bool CertificationsVerified { get; set; }
    public new double? Rating { get; set; }
    public new int NumberOfRatings { get; set; }
    public new int NumberOfLessons { get; set; }
    public new int OrderNumber { get; set; }
    public CSubjectVM Subject { get; set; }
    public AppUserStatisticsVM Statistics { get; set; }
    public string Timezone { get; set; }
    public CCountryVM Country { get; set; }
    public CCurrencyVM Currency { get; set; }
    public IEnumerable<UserAttributeVM> Attributes { get; set; }
    public IEnumerable<AppUserLanguageVM> Languages { get; set; }
    public IEnumerable<AppUserSpecializationVM> Specializations { get; set; }
    public IEnumerable<UserFileVM> Files { get; set; }
    public IEnumerable<CSubjectLevelVM> SubjectLevels { get; set; }
    public IEnumerable<CInterestVM> Interests { get; set; }
    [JsonIgnore] public new int? SubjectId { get; set; }
    [JsonIgnore] public new int? CountryId { get; set; }
    [JsonIgnore] public new IReadOnlyCollection<int> LanguageIds { get; set; }
    [JsonIgnore] public new int? FromTrialPrice { get; set; }
    [JsonIgnore] public new int? ToTrialPrice { get; set; }
}