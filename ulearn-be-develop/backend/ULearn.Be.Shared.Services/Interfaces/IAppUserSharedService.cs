using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppUser;

namespace ULearn.Be.Shared.Services.Interfaces;

public interface IAppUserSharedService
{
    Task<AppUserPublicVM> GetTutorPublicProfile(int userId, ClaimsPrincipal principal);
    Task<AppUserVM> GetPrivateProfile(ClaimsPrincipal principal);

    Task<PagedVM<AppUserOverviewVM>> Filter(
        FilterView<UserFilterVM> filter,
        CUserRoleEnum userRoleEnum,
        ClaimsPrincipal user
    );

    Task<PagedVM<AppRelatedUserVM>> GetTutors(ClaimsPrincipal user);
    Task<PagedVM<AppRelatedUserVM>> GetStudents(ClaimsPrincipal user);
}