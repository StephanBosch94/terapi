using ULearn.Be.Models.ViewModels.Auth;

namespace ULearn.Be.Providers.Auth.Interfaces;

public interface IGoogleApiProvider : IExternalAuthProvider
{
    Task<AuthUserFromExternalSourceVM> ValidateToken(string token);
}