using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppUser;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppUserRecommendationService
{
    Task<PagedVM<AppUserOverviewVM>> GetMatchingTutors(ClaimsPrincipal user, int take);
    Task<PagedVM<AppTutorCardVm>> PopularTutorsFilter(FilterView<AppTutorCardFilterVM> filter, ClaimsPrincipal user);
    Task<PagedVM<AppTutorCardVm>> SimilarTutorsFilter(int userId, FilterView<AppTutorCardFilterVM> filter, ClaimsPrincipal user);
}