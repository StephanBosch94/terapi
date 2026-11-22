using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppUserRecommendationService : IAppUserRecommendationService
{
    private readonly IAppUserSharedService _appUserSharedService;
    private readonly IAppUserRepository _appUserRepository;

    public AppUserRecommendationService(IAppUserSharedService appUserSharedService, IAppUserRepository appUserRepository)
    {
        _appUserSharedService = appUserSharedService;
        _appUserRepository = appUserRepository;
    }

    public async Task<PagedVM<AppUserOverviewVM>> GetMatchingTutors(ClaimsPrincipal user, int take)
    {
        var currentUserInfo = await _appUserSharedService.GetPrivateProfile(user);
        var subjectId = currentUserInfo.Subject?.SubjectId;
        var subjectLevelIds = currentUserInfo.SubjectLevels?.Select(x => x.SubjectLevelId).ToList();
        var specializationIds = currentUserInfo.Specializations?.Select(x => x.SpecializationId).ToList();
        var interestIds = currentUserInfo.Interests?.Select(x => x.InterestId).ToList();

        if (!subjectId.HasValue)
        {
            return new PagedVM<AppUserOverviewVM>();
        }

        var matching = await _appUserSharedService.Filter(
            new FilterView<UserFilterVM>
            {
                Skip = 0,
                Take = take,
                Filter = new UserFilterVM
                {
                    SubjectId = subjectId,
                    SubjectLevelIds = subjectLevelIds,
                    SpecializationIds = specializationIds,
                    InterestIds = interestIds
                }
            }, CUserRoleEnum.Tutor, user);

        var fallback2 = await _appUserSharedService.Filter(new FilterView<UserFilterVM>
        {
            Skip = 0,
            Take = take,
            Filter = new UserFilterVM
            {
                SubjectId = subjectId,
                SubjectLevelIds = null,
                SpecializationIds = specializationIds,
                InterestIds = interestIds
            }
        }, CUserRoleEnum.Tutor, user);

        var fallback3 = await _appUserSharedService.Filter(new FilterView<UserFilterVM>
        {
            Skip = 0,
            Take = take,
            Filter = new UserFilterVM
            {
                SubjectId = subjectId,
                SubjectLevelIds = null,
                SpecializationIds = specializationIds,
                InterestIds = null
            }
        }, CUserRoleEnum.Tutor, user);

        var subjectResult = await _appUserSharedService.Filter(new FilterView<UserFilterVM>
        {
            Skip = 0,
            Take = take,
            Filter = new UserFilterVM
            {
                SubjectId = subjectId,
                SubjectLevelIds = null,
                SpecializationIds = null,
                InterestIds = null
            }
        }, CUserRoleEnum.Tutor, user);

        var joined = new List<PagedVM<AppUserOverviewVM>>
            {
                matching,
                fallback2,
                fallback3,
                subjectResult
            }.SelectMany(x => x.Data)
            .Select((x, idx) => new { User = x, Idx = idx })
            .GroupBy(x => x.User.UserId)
            .Select(x => x.OrderBy(y => y.Idx).First())
            .OrderBy(x => x.Idx)
            .Select(x => x.User)
            .ToList();

        return new PagedVM<AppUserOverviewVM>(subjectResult.Size, joined);
    }

    public Task<PagedVM<AppTutorCardVm>> PopularTutorsFilter(FilterView<AppTutorCardFilterVM> filter, ClaimsPrincipal user)
    {
        return _appUserRepository.PopularTutorsFilter(filter);
    }

    public Task<PagedVM<AppTutorCardVm>> SimilarTutorsFilter(int userId, FilterView<AppTutorCardFilterVM> filter, ClaimsPrincipal user)
    {
        return _appUserRepository.SimilarTutorsFilter(userId, filter);
    }
}