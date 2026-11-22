using ULearn.Be.Models.ViewModels.AppUser.Attribute;
using ULearn.Be.Models.ViewModels.AppUser.Interfaces;
using ULearn.Be.Models.ViewModels.AppUser.Language;
using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.AppUser.Specialization;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppUser;

public record AppUserVM : AppUserBasicVM, IAppUserCollectionsVM
{
    public double? Rating { get; set; }
    public AppUserBillingInfoVM BillingInfo { get; set; }
    public CCurrencyVM Currency { get; set; }
    public CSubjectVM Subject { get; set; }
    public CCountryVM Country { get; set; }
    public CTeachingExperienceVM TeachingExperience { get; set; }
    public CWeeklyGoalVM WeeklyGoal { get; set; }
    public IEnumerable<UserAttributeVM> Attributes { get; set; }
    public IEnumerable<AppUserLanguageVM> Languages { get; set; }
    public IEnumerable<AppUserReviewVM> Reviews { get; set; }
    public IEnumerable<AppUserSpecializationVM> Specializations { get; set; }
    public IEnumerable<CSubjectLevelVM> SubjectLevels { get; set; }
    public IEnumerable<CInterestVM> Interests { get; set; }
}