using System.Security.Claims;
using ULearn.Be.Common.Constants;

namespace ULearn.Be.Common.Extensions;

public static class ClaimExtension
{
    public const string RoleClaimKey = "ROLES_CLAIM";
    public const string UserIdClaimKey = "USER_ID";
    public const string UserNameClaimKey = "USERNAME";
    public const string UserEmailClaimKey = "USER_EMAIL";
    public const string UserState = "USER_STATE";
    public const string UserRoleId = "USER_ROLE_ID";
    public const string RegistrationConfirmationClaim = "REGISTRATION_CONFIRMATION";
    public const int InvalidUserId = -1;

    public static string GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(UserEmailClaimKey)?.Value;
    }

    public static string GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(UserNameClaimKey)?.Value;
    }

    public static int GetUserId(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(UserIdClaimKey)?.Value ?? InvalidUserId.ToString();

        return Convert.ToInt32(userId);
    }

    public static bool IsStudent(this ClaimsPrincipal principal)
    {
        return principal.IsInRole(AppRoleConstants.Student);
    }

    public static bool IsTutor(this ClaimsPrincipal principal)
    {
        return principal.IsInRole(AppRoleConstants.Tutor);
    }

    public static bool IsAdmin(this ClaimsPrincipal principal)
    {
        return principal.IsInRole(AppRoleConstants.Admin);
    }
}