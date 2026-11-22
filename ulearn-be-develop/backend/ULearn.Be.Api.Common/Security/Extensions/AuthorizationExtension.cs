using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Api.Common.Security.Extensions;

public static class AuthorizationExtension
{
    public static void AddUserRoleAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(
            authorizationOptions =>
            {
                authorizationOptions.FallbackPolicy = authorizationOptions.DefaultPolicy;

                authorizationOptions.AddPolicy(
                    PolicyConstants.AnyRole,
                    policy => policy.RequireRole(
                        AppRoleConstants.Tutor,
                        AppRoleConstants.Student,
                        AppRoleConstants.Admin,
                        AppRoleConstants.Company
                    )
                );

                authorizationOptions.AddPolicy(
                    PolicyConstants.OnlyAdmin,
                    policy => policy.RequireRole(
                        AppRoleConstants.Admin
                    )
                );

                authorizationOptions.AddPolicy(
                    PolicyConstants.OnlyTutor,
                    policy => policy.RequireRole(
                        AppRoleConstants.Admin,
                        AppRoleConstants.Tutor
                    )
                );

                authorizationOptions.AddPolicy(
                    PolicyConstants.OnlyStudent,
                    policy => policy.RequireRole(
                        AppRoleConstants.Admin,
                        AppRoleConstants.Student
                    )
                );

                authorizationOptions.AddPolicy(
                    PolicyConstants.OnlyCompany,
                    policy => policy.RequireRole(
                        AppRoleConstants.Admin,
                        AppRoleConstants.Company
                    )
                );
                
                authorizationOptions.AddPolicy(
                    PolicyConstants.RegistrationCodeConfirmation,
                    policy => policy.RequireClaim(ClaimExtension.RegistrationConfirmationClaim)
                );
            }
        );
    }
}