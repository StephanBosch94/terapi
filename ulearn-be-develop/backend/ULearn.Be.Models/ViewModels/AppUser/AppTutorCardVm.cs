using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.ViewModels.AppUser;

public class AppTutorCardVm : AppTutorCardFilterVM
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public new DateTime Created { get; set; }
    public int StateId { get; set; }
    public int RoleId { get; set; }
    public double? HourRate { get; set; }
    public bool CertificationsVerified { get; set; }
    public new double? Rating { get; set; }
    public new int NumberOfRatings { get; set; }
    public new int NumberOfLessons { get; set; }
    public new int OrderNumber { get; set; }
    public bool HasConfiguredAvailability { get; set; }
    public string Timezone { get; set; }
    public CSubjectVM Subject { get; set; }
    public CCountryVM Country { get; set; }
    public CCurrencyVM Currency { get; set; }
    public IEnumerable<UserFileVM> Files { get; set; }
}