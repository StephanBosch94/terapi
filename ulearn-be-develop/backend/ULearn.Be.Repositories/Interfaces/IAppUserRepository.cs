using ULearn.Be.Common.Models;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.AppUser.Sitemap;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Models.ViewModels.Statistics;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserRepository
{
    Task<PagedVM<AppUserOverviewVM>> Filter(
        FilterView<UserFilterVM> filter,
        CUserRoleEnum userRoleEnum,
        bool isAdmin
    );

    Task<PagedVM<AppTutorCardVm>> PopularTutorsFilter(FilterView<AppTutorCardFilterVM> filter);

    Task<PagedVM<AppTutorCardVm>> SimilarTutorsFilter(int userId, FilterView<AppTutorCardFilterVM> filter);

    Task<AppUserPublicVM> GetPublicInfo(int userId, bool onlyConfirmed = true);

    Task<AppUserVM> Get(int userId, CUserRoleEnum? role = null, bool onlyConfirmed = false,
        bool throwIfNotFound = true);

    Task<AppUserBasicVM> GetByLogin(string login, bool onlyConfirmed = false, bool throwIfNotFound = true);

    Task<AppUserBasicVM> GetById(
        int userId,
        bool onlyConfirmed = false,
        bool throwIfNotFound = true,
        bool includeDeleted = false
    );

    Task<bool> Exists(string login);
    Task<bool> ExistsEmployee(int adminId, int employeeId);
    Task<int> AddIfNotExists(RegistrationIM registrationIm);

    Task<AppUser> Upsert(
        AppUserIM userIm,
        int userId,
        int? inviterId = null
    );

    Task<(AppUser user, bool created)> AddIfNotExists(
        AppUserIM registrationIm,
        string login,
        int? inviterId = null
    );

    Task UpdateState(int userId, CUserStateEnum stateEnum);
    Task UpdateHubspotInfo(int userId, string hubspotContactId);
    Task<AppUser> GetRawDbEntity(string login, bool throwIfNotFound = true);
    Task<AppUser> GetRawDbEntity(int userId, bool throwIfNotFound = true, bool includeDeleted = false);
    Task<UserPriceStatisticsVM> GetPriceStatistics();
    Task<Dictionary<int, AppUserCommonVM>> GetUserDictionary(IEnumerable<int> userIds);
    Task<double> GetHourRate(int tutorId);
    Task<int> GetSubjectId(int tutorId);
    Task<CUserRoleEnum> GetRoleId(int userId);
    Task Approve(int userId);
    Task Refuse(int userId);
    Task ChangeLanguage(int translationLanguageEnum, int userId);
    Task UpdatePassword(PasswordUpdateIM input, int userId);
    Task<Dictionary<int, CSubjectVM>> GetUserSubjects(IEnumerable<int> userIds);
    Task<AppUserInfoForNotificationVM> GetNotificationInfo(int userId);
    Task<PagedVM<AppUserEmployeeVM>> GetEmployeeFilter(int adminId, FilterView<AppUserEmployeeFilterVM> filter);
    Task<IReadOnlyCollection<int>> GetEmployeeIds(int adminId);
    Task BatchHardDelete(AppUserAdminBatchDeleteIM input);
    Task<List<AppUserSitemapVM>> GetTutorSitemapInfo();
    Task<AppTutorAdminInfoVM> GetTutorAdminInfo(int userId);
    Task<int> GetNumberOfEmployees(int userId);
    Task<IReadOnlyDictionary<int, int>> GetEmployeesGoalsByUserId(int userId);
    Task UpdateEmployeeAccount(AppEmployeeIM input);
    Task AdminUpdate(AppUserAdminIM input);
    Task<IReadOnlyCollection<int>> GetWelcomeEmailUserIdsToSend();

    Task<IReadOnlyCollection<int>> GetDiscountCouponUserIdsToSend(
        CCouponTypeEnum couponTypeEnum,
        int discountPercentage,
        int durationInDaysAfterRegistration
    );

    Task UpdateCouponLastSendTime(IReadOnlyCollection<int> userIds);
}