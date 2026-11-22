using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ITokenService
{
    SecurityToken GetToken(IEnumerable<Claim> authClaims);
    CookieOptions GetCookieOptions(DateTimeOffset cookieExpireTime);
}