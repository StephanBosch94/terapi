using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.AppUser;

namespace ULearn.Be.Services.Utils;

public static class AuthUtil
{
    public static ICollection<Claim> GetClaims(AppUserBasicVM appUserVm) =>
        GetClaims(appUserVm.UserId, appUserVm.UserRoleId, appUserVm.StateId);

    public static ICollection<Claim> GetClaims(int userId, int userRoleId, int stateId)
    {
        return new List<Claim>
        {
            new(ClaimExtension.UserIdClaimKey, userId.ToString()),
            new(ClaimTypes.Role, ((CUserRoleEnum)userRoleId).GetEnumKey()),
            new(ClaimExtension.UserRoleId, userRoleId.ToString()),
            new(ClaimExtension.RoleClaimKey, ((CUserRoleEnum)userRoleId).GetEnumKey()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimExtension.UserState, stateId.ToString())
        };
    }
}