using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppNotification;
using ULearn.Be.Models.ViewModels.AppUser.Review;
using ULearn.Be.Models.ViewModels.AppUserReview;

namespace ULearn.Be.Models.Mappers;

public static class AppUserReviewMapper
{
    public static readonly Expression<Func<AppUserReview, AppUserReviewVM>> AsVM =
        dataItem => new AppUserReviewVM
        {
            Comment = dataItem.Comment,
            Created = dataItem.Created,
            Updated = dataItem.Updated,
            Rate = dataItem.Rate,
            UserReviewId = dataItem.UserReviewId,
            Reviewer = AppUserMapper.ToCommonVm(dataItem.Reviewer),
            RevieweeId = dataItem.RevieweeId,
            ReviewerId = dataItem.ReviewerId
        };

    public static readonly Expression<Func<AppUserReview, AppUserReviewForNotificationVM>>
        AsAppUserReviewForNotificationVM =
            dataItem => new AppUserReviewForNotificationVM
            {
                Rate = dataItem.Rate,
                UserReviewId = dataItem.UserReviewId,
                Comment = dataItem.Comment,
                Reviewer = AppUserMapper.ToCommonVm(dataItem.Reviewer),
                Reviewee = AppUserMapper.ToCommonVm(dataItem.Reviewee)
            };

    public static AppNotificationReviewVM ToNotificationVM(AppUserReview dataItem)
    {
        return new AppNotificationReviewVM
        {
            Comment = dataItem.Comment,
            Rate = dataItem.Rate
        };
    }

    public static IQueryable<AppUserReviewVM> ToApiList(this IQueryable<AppUserReview> dataItems)
    {
        return dataItems
            .Include(x => x.Reviewer)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsVM);
    }

    public static IQueryable<AppUserReviewForNotificationVM> ToAppUserReviewForNotificationVmList(
        this IQueryable<AppUserReview> dataItems)
    {
        return dataItems
            .Include(x => x.Reviewer)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Reviewee)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppUserReviewForNotificationVM);
    }
}