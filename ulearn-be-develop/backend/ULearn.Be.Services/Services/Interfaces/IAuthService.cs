using System.Security.Claims;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.Auth;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAuthService
{
    Task<ICollection<Claim>> LoginGoogle(AuthExternalSignUpVM input);
    Task<ICollection<Claim>> LoginGoogleOneTap(GoogleOneTapLoginIM input);
    Task<ICollection<Claim>> LoginFacebook(AuthExternalSignUpVM input);
    Task<ICollection<Claim>> LoginSeznam(AuthExternalSignUpVM input);
    Task<ICollection<Claim>> LoginInternal(AuthInternalLoginIM input);
    Task<AuthTokenVM> LoginInternalBearer(AuthInternalLoginIM authInternalLoginIm);
}