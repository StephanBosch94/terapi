using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.AppUser.Sitemap;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppUserService
{
    Task<AppUserPublicVM> GetTutorPublicProfile(int userId, ClaimsPrincipal principal);
    Task<AppUserVM> GetPrivateProfile(ClaimsPrincipal principal);
    Task<ICollection<Claim>> Update(AppUserIM appUserIm, int userId, ClaimsPrincipal principal);

    Task<PagedVM<AppUserOverviewVM>> UserFilter(
        FilterView<UserFilterVM> filter,
        CUserRoleEnum userRoleEnum,
        ClaimsPrincipal user
    );

    Task<AppShortUserInfoVM> GetShortInfo(ClaimsPrincipal user);
    Task<PagedVM<AppRelatedUserVM>> GetStudents(ClaimsPrincipal user);
    Task<PagedVM<AppRelatedUserVM>> GetTutors(ClaimsPrincipal user);

    Task<PagedVM<AppUserEmployeeVM>> EmployeesFilter(
        ClaimsPrincipal user,
        FilterView<AppUserEmployeeFilterVM> filter
    );

    Task<AppTutorAdminInfoVM> GetTutorAdminInfo(int userId, ClaimsPrincipal user);

    Task Approve(int userId, ClaimsPrincipal user);
    Task Refuse(int userId, ClaimsPrincipal user);
    Task ChangeTranslationLanguage(string languageCode, ClaimsPrincipal user);
    Task<AppUserBasicVM> UpdatePassword(PasswordUpdateIM input, ClaimsPrincipal user);
    Task DeleteEmployeeAccount(AppUserDeleteEmployeeIM input, ClaimsPrincipal user);
    Task BatchHardAdminDelete(AppUserAdminBatchDeleteIM input, ClaimsPrincipal user);
    Task<List<AppUserSitemapVM>> GetTutorsSitemapInfo(ClaimsPrincipal user);
    Task UpdateEmployeeAccount(AppEmployeeIM input, ClaimsPrincipal user);
    Task AdminUpdate(AppUserAdminIM input, ClaimsPrincipal user);
}