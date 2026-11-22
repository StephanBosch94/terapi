using System.Security.Claims;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.AppUserStatistics;
using ULearn.Be.Models.ViewModels.AppUser.Interfaces;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class AppUserSharedService : IAppUserSharedService
{
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppUserBehaviourRepository _appUserBehaviourRepository;
    private readonly IAppUserReviewRepository _appUserReviewRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IRUserAttributeRepository _rUserAttributeRepository;
    private readonly IRUserFileRepository _rUserFileRepository;
    private readonly IRUserInterestRepository _rUserInterestRepository;
    private readonly IRUserLanguageRepository _rUserLanguageRepository;
    private readonly IRUserSpecializationRepository _rUserSpecializationRepository;
    private readonly IRUserSubjectLevelRepository _rUserSubjectLevelRepository;
    private readonly IAppUserRepository _userRepository;

    public AppUserSharedService(
        IAppUserBehaviourRepository appUserBehaviourRepository,
        IAppReservationRepository appReservationRepository,
        IRUserAttributeRepository rUserAttributeRepository,
        IRUserFileRepository rUserFileRepository,
        IRUserLanguageRepository rUserLanguageRepository,
        IRUserSpecializationRepository rUserSpecializationRepository,
        IRUserSubjectLevelRepository rUserSubjectLevelRepository,
        IAppUserRepository userRepository,
        IAppUserReviewRepository appUserReviewRepository,
        IRUserInterestRepository rUserInterestRepository,
        IAppUserWalletRepository appUserWalletRepository
    )
    {
        _appUserBehaviourRepository = appUserBehaviourRepository;
        _appReservationRepository = appReservationRepository;
        _rUserAttributeRepository = rUserAttributeRepository;
        _rUserFileRepository = rUserFileRepository;
        _rUserLanguageRepository = rUserLanguageRepository;
        _rUserSpecializationRepository = rUserSpecializationRepository;
        _rUserSubjectLevelRepository = rUserSubjectLevelRepository;
        _userRepository = userRepository;
        _appUserReviewRepository = appUserReviewRepository;
        _rUserInterestRepository = rUserInterestRepository;
        _appUserWalletRepository = appUserWalletRepository;
    }

    public async Task<AppUserPublicVM> GetTutorPublicProfile(int userId, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userRepository.GetPublicInfo(userId, onlyConfirmed: claimsPrincipal.IsAdmin());
        await EnhanceWithPublicProps(user);
        return user;
    }

    public async Task<AppUserVM> GetPrivateProfile(ClaimsPrincipal principal)
    {
        var user = await _userRepository.Get(principal.GetUserId(), onlyConfirmed: false);
        await EnhanceWithPublicProps(user);
        await EnhanceWithPrivateProps(user);
        return user;
    }

    public async Task<PagedVM<AppUserOverviewVM>> Filter(
        FilterView<UserFilterVM> filter,
        CUserRoleEnum userRoleEnum,
        ClaimsPrincipal user
    )
    {
        ThrowIfForbiddenStateFiltering(filter, user.IsAdmin());
        var result = await _userRepository.Filter(filter, userRoleEnum, user.IsAdmin());
        await EnhanceVMs(result.Data);
        return result;
    }

    public async Task<PagedVM<AppRelatedUserVM>> GetTutors(ClaimsPrincipal user)
    {
        var users = await _appReservationRepository.GetTutors(user.GetUserId());
        var results = new PagedVM<AppRelatedUserVM>(users.Data.Select(x => new AppRelatedUserVM(x)));
        await EnhanceVMs(results.Data, user);
        return results;
    }

    public async Task<PagedVM<AppRelatedUserVM>> GetStudents(ClaimsPrincipal user)
    {
        var users = await _appReservationRepository.GetStudents(user.GetUserId());
        var results = new PagedVM<AppRelatedUserVM>(users.Data.Select(x => new AppRelatedUserVM(x)));
        await EnhanceVMs(results.Data, user);
        return results;
    }

    private async Task<IReadOnlyDictionary<int, AppUserStatisticsVM>> GetStatisticsByUserId(
        IReadOnlyCollection<int> userIds
    )
    {
        var onlineUserIds = await _appUserBehaviourRepository.GetOnlineUserIds(userIds);
        var lastVisitedByUserId = await _appUserBehaviourRepository.GetLastVisitedByUserId(userIds);

        return userIds.Select(userId => new
        {
            UserId = userId,
            Statistics = new AppUserStatisticsVM
            {
                IsOnline = onlineUserIds.Contains(userId),
                LastVisited = lastVisitedByUserId.ContainsKey(userId) ? lastVisitedByUserId[userId] : null,
            }
        }).ToDictionary(x => x.UserId, x => x.Statistics);
    }

    private static void ThrowIfForbiddenStateFiltering(FilterView<UserFilterVM> filter, bool isAdmin)
    {
        if (isAdmin)
        {
            return;
        }

        if (filter.Filter.StateId == null)
        {
            return;
        }

        if (filter.Filter.StateId == (int)CUserStateEnum.Confirmed)
        {
            return;
        }

        throw new ForbiddenException();
    }

    private async Task EnhanceWithPublicProps(IAppUserCollectionsVM user)
    {
        user.Attributes = await _rUserAttributeRepository.GetByUserId(user.UserId);
        user.Languages = await _rUserLanguageRepository.GetByUserId(user.UserId);
        user.Specializations = await _rUserSpecializationRepository.GetByUserId(user.UserId);
        user.Files = await _rUserFileRepository.GetByUserId(user.UserId);
        user.SubjectLevels = await _rUserSubjectLevelRepository.GetByUserId(user.UserId);
        user.Interests = await _rUserInterestRepository.GetByUserId(user.UserId);
        user.Reviews = user.UserRoleId == (int)CUserRoleEnum.Tutor
            ? await _appUserReviewRepository.GetByRevieweeId(user.UserId)
            : null;
    }

    private async Task EnhanceVMs(ICollection<AppUserOverviewVM> users)
    {
        var userIds = users.Select(x => x.UserId).ToHashSet();
        var attributesByUserId = await _rUserAttributeRepository.GetAttributesByUserId(userIds);
        var languagesByUserId = await _rUserLanguageRepository.GetLanguagesByUserId(userIds);
        var specializationsByUserId = await _rUserSpecializationRepository.GetSpecializationsByUserId(userIds);
        var filesByUserId = await _rUserFileRepository.GetFilesByUserId(userIds);
        var subjectLevelsByUserId = await _rUserSubjectLevelRepository.GetSubjectLevelsByUserId(userIds);
        var interestByUserId = await _rUserInterestRepository.GetInterestByUserId(userIds);
        var statisticsByUserId = await GetStatisticsByUserId(userIds);

        foreach (var appUserOverviewVm in users)
        {
            appUserOverviewVm.Attributes = attributesByUserId[appUserOverviewVm.UserId];
            appUserOverviewVm.Languages = languagesByUserId[appUserOverviewVm.UserId];
            appUserOverviewVm.Specializations = specializationsByUserId[appUserOverviewVm.UserId];
            appUserOverviewVm.Files = filesByUserId[appUserOverviewVm.UserId];
            appUserOverviewVm.SubjectLevels = subjectLevelsByUserId[appUserOverviewVm.UserId];
            appUserOverviewVm.Interests = interestByUserId[appUserOverviewVm.UserId];
            appUserOverviewVm.Statistics = appUserOverviewVm.RoleId == (int)CUserRoleEnum.Tutor &&
                                           statisticsByUserId.ContainsKey(appUserOverviewVm.UserId)
                ? statisticsByUserId[appUserOverviewVm.UserId]
                : null;
        }
    }

    private async Task EnhanceVMs(ICollection<AppRelatedUserVM> users, ClaimsPrincipal currentUser)
    {
        var currentUserId = currentUser.GetUserId();
        var isTutor = currentUser.IsInRole(CUserRoleEnum.Tutor.GetEnumKey());
        var userIds = users.Select(x => x.UserId).ToHashSet();
        var filesByUserId = await _rUserFileRepository.GetFilesByUserId(userIds);
        var userSubjects = await _userRepository.GetUserSubjects(userIds);
        var userReviews =
            (await _appUserReviewRepository.GetByReviewerId(currentUserId)).ToLookup(x => x.RevieweeId);
        var numberOfCompletedLessonByUserId = isTutor
            ? await _appReservationRepository.GetTutorNumberOfCompletedLessonsByStudentId(currentUserId)
            : await _appReservationRepository.GetStudentNumberOfCompletedLessonsByTutorId(currentUserId);

        foreach (var user in users)
        {
            user.Files = filesByUserId[user.UserId];
            user.UserReview = userReviews[user.UserId].FirstOrDefault();
            user.Subject = userSubjects.TryGetValue(user.UserId, out var subject) ? subject : null;
            user.NumberOfCompletedLessons = numberOfCompletedLessonByUserId.ContainsKey(user.UserId)
                ? numberOfCompletedLessonByUserId[user.UserId]
                : 0;
        }
    }

    private async Task EnhanceWithPrivateProps(AppUserVM user)
    {
        user.BillingInfo = await _appUserWalletRepository.GetBillingInfo(user.UserId);
    }
}