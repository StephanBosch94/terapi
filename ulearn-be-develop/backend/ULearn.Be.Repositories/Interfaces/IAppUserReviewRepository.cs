using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.AppUserReview;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserReviewRepository
{
    Task<IEnumerable<AppUserReviewVM>> GetByRevieweeId(int userId);
    Task<IEnumerable<AppUserReviewVM>> GetByReviewerId(int userId);
    Task<int> Add(AppUserReviewIM input, int userId);
    Task Edit(AppUserReviewUpdateIM input, int userId);
    Task<AppUserReviewForNotificationVM> GetNotificationVmById(int userReviewId);
}