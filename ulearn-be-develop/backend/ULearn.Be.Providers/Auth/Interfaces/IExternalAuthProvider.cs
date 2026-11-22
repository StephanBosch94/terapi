using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.Auth;

namespace ULearn.Be.Providers.Auth.Interfaces;

public interface IExternalAuthProvider
{
    Task<AuthUserFromExternalSourceVM> Authenticate(AuthExternalLoginIM input);
}