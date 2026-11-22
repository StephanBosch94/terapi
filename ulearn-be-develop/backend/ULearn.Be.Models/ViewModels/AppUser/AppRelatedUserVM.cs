using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppUser;

public record AppRelatedUserVM : AppUserCommonVM
{
    public AppRelatedUserVM(AppUserCommonVM userCommonVm) : base(userCommonVm)
    {
    }

    public CSubjectVM Subject { get; set; }
    public AppUserReviewVM UserReview { get; set; }
    public int NumberOfCompletedLessons { get; set; }
}