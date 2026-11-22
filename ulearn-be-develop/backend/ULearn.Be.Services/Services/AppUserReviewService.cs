using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppUserReviewService : IAppUserReviewService
{
    private readonly IAppUserReviewRepository _appUserReviewRepository;
    private readonly INotificationSharedService _notificationSharedService;

    public AppUserReviewService(
        IAppUserReviewRepository appUserReviewRepository,
        INotificationSharedService notificationSharedService
    )
    {
        _appUserReviewRepository = appUserReviewRepository;
        _notificationSharedService = notificationSharedService;
    }

    public async Task Add(AppUserReviewIM input, ClaimsPrincipal user)
    {
        var userReviewId = await _appUserReviewRepository.Add(input, user.GetUserId());
        await _notificationSharedService.SendNewUserReview(userReviewId);
    }

    public Task Edit(AppUserReviewUpdateIM input, ClaimsPrincipal user)
    {
        return _appUserReviewRepository.Edit(input, user.GetUserId());
    }
}