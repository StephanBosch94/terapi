using ULearn.Be.Models.ViewModels.AppUser.Attribute;
using ULearn.Be.Models.ViewModels.AppUser.Language;
using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.AppUser.Specialization;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.ViewModels.AppUser.Interfaces;

public interface IAppUserCollectionsVM
{
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