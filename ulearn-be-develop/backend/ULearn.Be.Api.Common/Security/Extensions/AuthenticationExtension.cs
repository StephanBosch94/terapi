using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;

namespace ULearn.Be.Api.Common.Security.Extensions;

public static class AuthenticationExtension
{
    public static void AddCookieAuthentication(this IServiceCollection services, string authSettingsDomain)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Cookie.Name = CommonConstants.AuthCookieName;
                    options.Cookie.HttpOnly = true;
                    options.SlidingExpiration = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.Domain = authSettingsDomain;
                });
    }

    public static void AddJwtAuthentication(this IServiceCollection services, string secret)
    {
        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(
            options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        var roles = ctx.Principal.FindAll(ClaimExtension.RoleClaimKey);
                        var claims = new List<Claim>();

                        foreach (var role in roles)
                        {
                            switch (role.Value)
                            {
                                case AppRoleConstants.Admin:
                                    claims.Add(new Claim(ClaimTypes.Role, AppRoleConstants.Admin));
                                    break;
                                case AppRoleConstants.Student:
                                    claims.Add(new Claim(ClaimTypes.Role, AppRoleConstants.Student));
                                    break;
                                case AppRoleConstants.Tutor:
                                    claims.Add(new Claim(ClaimTypes.Role, AppRoleConstants.Tutor));
                                    break;
                            }
                        }

                        ctx.Principal.AddIdentity(new ClaimsIdentity(claims));

                        return Task.CompletedTask;
                    }
                };
            }
        );
    }
}