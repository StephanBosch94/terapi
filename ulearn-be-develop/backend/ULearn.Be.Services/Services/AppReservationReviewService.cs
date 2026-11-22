using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppReservationReviewService : IAppReservationReviewService
{
    private readonly IAppUserReservationReviewRepository _appUserReservationReviewRepository;

    public AppReservationReviewService(IAppUserReservationReviewRepository appUserReservationReviewRepository)
    {
        _appUserReservationReviewRepository = appUserReservationReviewRepository;
    }

    public Task Add(AppReservationReviewIM input, ClaimsPrincipal user)
    {
        return _appUserReservationReviewRepository.Add(input, user.GetUserId());
    }

    public Task Edit(AppReservationReviewEditIM input, ClaimsPrincipal user)
    {
        return _appUserReservationReviewRepository.Edit(input, user.GetUserId());
    }
}