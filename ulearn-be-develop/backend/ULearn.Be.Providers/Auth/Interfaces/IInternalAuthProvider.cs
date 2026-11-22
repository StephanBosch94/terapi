using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Providers.Auth.Interfaces;

public interface IInternalAuthProvider
{
    Task Authenticate(AuthInternalLoginIM input);
}