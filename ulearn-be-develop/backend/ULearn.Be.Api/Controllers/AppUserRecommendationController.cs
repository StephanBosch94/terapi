using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-recommendation")]
public class AppUserRecommendationController : CustomControllerBase
{
    private readonly IAppUserRecommendationService _appUserRecommendationService;
    private readonly ILogger<AppUserRecommendationController> _logger;

    public AppUserRecommendationController(
        ITokenService tokenService,
        IAppUserRecommendationService appUserRecommendationService,
        ILogger<AppUserRecommendationController> logger
    ) : base(tokenService)
    {
        _appUserRecommendationService = appUserRecommendationService;
        _logger = logger;
    }

    [HttpGet("matching-tutors")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppUserOverviewVM>), StatusCodes.Status200OK)]
    public Task<PagedVM<AppUserOverviewVM>> GetMatchingTutors([FromQuery] int? take)
    {
        return _appUserRecommendationService.GetMatchingTutors(User, take ?? CommonConstants.FilterViewDefaultTake);
    }
    
    [HttpPost("popular-tutors/filter")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppTutorCardVm>), StatusCodes.Status200OK)]
    public Task<PagedVM<AppTutorCardVm>> PopularTutorFilter(
        FilterView<AppTutorCardFilterVM> filter
    )
    {
        return _appUserRecommendationService.PopularTutorsFilter(filter, User);
    }
    
    [HttpPost("tutor/{userId:int}/similar-tutors/filter")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppTutorCardVm>), StatusCodes.Status200OK)]
    public Task<PagedVM<AppTutorCardVm>> SimilarTutorFilter(
        FilterView<AppTutorCardFilterVM> filter,
        int userId
    )
    {
        return _appUserRecommendationService.SimilarTutorsFilter(userId, filter, User);
    }
    
}