using Newtonsoft.Json;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppUser.Review;

public class AppUserReviewVM
{
    public int UserReviewId { get; set; }
    public int Rate { get; set; }
    public string Comment { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public AppUserCommonVM Reviewer { get; set; }
    [JsonIgnore] public int ReviewerId { get; set; }
    [JsonIgnore] public int RevieweeId { get; set; }
}