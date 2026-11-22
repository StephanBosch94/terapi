using ULearn.Be.Models.ViewModels.AppUser.Attribute;
using ULearn.Be.Models.ViewModels.AppUser.Interfaces;
using ULearn.Be.Models.ViewModels.AppUser.Language;
using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.AppUser.Specialization;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.ViewModels.AppUser;

public record AppUserPublicVM : IAppUserCollectionsVM
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Timezone { get; set; }
    public double? HourRate { get; set; }
    public int? SubjectId { get; set; }
    public double? Rating { get; set; }
    public bool CertificationsVerified { get; set; }
    public bool TeachesLessonsForChildren { get; set; }
    public CCurrencyVM Currency { get; set; }
    public CSubjectVM Subject { get; set; }
    public CCountryVM Country { get; set; }
    public CTeachingExperienceVM TeachingExperience { get; set; }
    public CWeeklyGoalVM WeeklyGoal { get; set; }
    public int UserId { get; set; }
    public int UserRoleId { get; set; }
    public IEnumerable<UserAttributeVM> Attributes { get; set; }
    public IEnumerable<AppUserLanguageVM> Languages { get; set; }
    public IEnumerable<AppUserReviewVM> Reviews { get; set; }
    public IEnumerable<AppUserSpecializationVM> Specializations { get; set; }
    public IEnumerable<CSubjectLevelVM> SubjectLevels { get; set; }
    public IEnumerable<CInterestVM> Interests { get; set; }
    public IEnumerable<UserFileVM> Files { get; set; }
}