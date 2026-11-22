using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppUserReview;

public class AppUserReviewForNotificationVM
{
    public int UserReviewId { get; set; }
    public int Rate { get; set; }
    public string Comment { get; set; }
    public AppUserCommonVM Reviewer { get; set; }
    public AppUserCommonVM Reviewee { get; set; }
}