using System.Security.Claims;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUserRegistration;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IRegistrationService
{
    Task<ICollection<Claim>> Register(RegistrationIM registrationIm);

    Task<ICollection<Claim>> Activate(
        RegistrationActivationIM registrationActivationIm,
        ClaimsPrincipal claimsPrincipal
    );

    Task<UserRegistrationCodeInfoVM> ResendCode(ClaimsPrincipal principal);
    Task<UserRegistrationCodeInfoVM> GetLatestCodeInfo(ClaimsPrincipal user);
    Task ForgottenPassword(ForgottenPasswordInputVM input);
    Task InviteEmployees(AppUserInviteIM input, ClaimsPrincipal user);
}