using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUserRegistration;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Utils;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IAppUserRegistrationCodeRepository _appUserRegistrationCodeRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IGuidUtility _guidUtility;
    private readonly IHubspotSharedService _hubspotSharedService;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly ITimeUtility _timeUtility;
    private readonly ITokenService _tokenService;

    public RegistrationService(
        INotificationSharedService notificationSharedService,
        IAppUserRegistrationCodeRepository appUserRegistrationCodeRepository,
        IAppUserRepository appUserRepository,
        ITokenService tokenService,
        ITimeUtility timeUtility,
        IHubspotSharedService hubspotSharedService,
        IGuidUtility guidUtility
    )
    {
        _notificationSharedService = notificationSharedService;
        _appUserRegistrationCodeRepository = appUserRegistrationCodeRepository;
        _appUserRepository = appUserRepository;
        _tokenService = tokenService;
        _timeUtility = timeUtility;
        _hubspotSharedService = hubspotSharedService;
        _guidUtility = guidUtility;
    }

    public async Task InviteEmployees(AppUserInviteIM input, ClaimsPrincipal user)
    {
        var userId = user.GetUserId();

        var inputVms = input.Emails.Select(email =>
            new AppUserIM
            {
                Login = email,
                Email = email,
                UserRoleId = (int)CUserRoleEnum.Student,
                UserStateId = (int)CUserStateEnum.EmailVerification,
                AuthenticationMethodId = (int)CAuthenticationMethodEnum.Internal,
                WeeklyGoalId = (int?)input.WeeklyGoalId
            }).ToList();

        foreach (var appUserIm in inputVms)
        {
            var (userEntity, created) =
                await _appUserRepository.AddIfNotExists(appUserIm, appUserIm.Login, inviterId: userId);
            await _hubspotSharedService.SyncUser(userEntity.UserId);
            if (created)
            {
                var (code, token) = await GenerateShortRegistrationToken(userEntity.UserId);
                await _notificationSharedService.SendInvite(userEntity.UserId, code, token);
            }
        }

        await _notificationSharedService.SendCompanyInvitedEmployees(userId, inputVms.Count);
    }

    public async Task<ICollection<Claim>> Register(RegistrationIM registrationIm)
    {
        await ThrowIfUserExists(registrationIm.Email);
        var userId = await _appUserRepository.AddIfNotExists(registrationIm);
        var (code, token) = await GenerateShortRegistrationToken(userId);
        await _hubspotSharedService.SyncUser(userId);
        await _notificationSharedService.SendRegistrationCode(userId, code, token);
        var userVm = await _appUserRepository.GetByLogin(registrationIm.Email, onlyConfirmed: false);
        await _notificationSharedService.SendAccountCreated(userId);
        return AuthUtil.GetClaims(userVm);
    }

    public async Task<ICollection<Claim>> Activate(
        RegistrationActivationIM registrationActivationIm,
        ClaimsPrincipal claimsPrincipal
    )
    {
        var dbEntity = await _appUserRegistrationCodeRepository
            .GetRawDbEntity(claimsPrincipal.GetUserId(), registrationActivationIm.RegistrationCode);

        if (dbEntity == null)
        {
            throw new ForbiddenException(ErrorCodesEnum.CodeDoesNotExistsOrInvalid);
        }

        await _appUserRegistrationCodeRepository.SetUsed(dbEntity.UserRegistrationCodeId);
        await _appUserRepository.UpdateState(dbEntity.UserId, CUserStateEnum.PasswordSetup);
        await _hubspotSharedService.SyncUser(dbEntity.UserId);
        var userVm = await _appUserRepository.GetById(claimsPrincipal.GetUserId(), onlyConfirmed: false);
        return AuthUtil.GetClaims(userVm);
    }

    public async Task<UserRegistrationCodeInfoVM> ResendCode(ClaimsPrincipal principal)
    {
        var userVm = await _appUserRepository.GetById(principal.GetUserId(), onlyConfirmed: false);
        ThrowIfNotInCreatedState(userVm);
        await ThrowIfSendingMultipleTimesInRow(userVm.UserId);
        var registrationToken = new JwtSecurityTokenHandler().WriteToken(GetRegistrationToken(userVm.UserId));
        var userRegistrationCode = _guidUtility.GenerateShortCode();
        await _appUserRegistrationCodeRepository.Add(userVm.UserId,
            userRegistrationCode); // add code for preventing user sending multiple times in a row
        await _notificationSharedService.SendRegistrationCode(principal.GetUserId(), userRegistrationCode,
            registrationToken);
        return await _appUserRegistrationCodeRepository.GetLatestCodeInfo(principal.GetUserId());
    }

    public Task<UserRegistrationCodeInfoVM> GetLatestCodeInfo(ClaimsPrincipal user)
    {
        return _appUserRegistrationCodeRepository.GetLatestCodeInfo(user.GetUserId());
    }

    public async Task ForgottenPassword(ForgottenPasswordInputVM input)
    {
        var userVm = await _appUserRepository.GetByLogin(input.Email, onlyConfirmed: false);
        if (userVm == null)
        {
            return;
        }

        await ThrowIfSendingMultipleTimesInRow(userVm.UserId);
        var registrationToken = new JwtSecurityTokenHandler().WriteToken(GetRegistrationToken(userVm.UserId));
        // add code for preventing user sending multiple times in a row
        await _appUserRegistrationCodeRepository.Add(userVm.UserId, _guidUtility.GenerateCode());
        await _notificationSharedService.SendForgottenPassword(
            userVm.UserId,
            registrationToken
        );
    }

    private async Task<(string tokenCode, string token)> GenerateShortRegistrationToken(int userId)
    {
        var code = _guidUtility.GenerateShortCode();
        var registrationToken = new JwtSecurityTokenHandler().WriteToken(GetRegistrationToken(userId));
        await _appUserRegistrationCodeRepository.Add(userId, code);
        return (code, registrationToken);
    }

    private async Task ThrowIfSendingMultipleTimesInRow(int userId)
    {
        var latestCodeInfo = await _appUserRegistrationCodeRepository.GetLatestCodeInfo(userId);
        if (latestCodeInfo == null)
        {
            return;
        }

        var diff = _timeUtility.GetUtcNow() - latestCodeInfo.Created;

        if (diff.TotalSeconds < CommonConstants.EmailVerificationTimer)
        {
            throw new ForbiddenException();
        }
    }

    private void ThrowIfNotInCreatedState(AppUserBasicVM userVm)
    {
        var stateId = userVm.StateId;
        if (stateId != (int)CUserStateEnum.EmailVerification)
        {
            throw new ConflictException();
        }
    }

    private async Task ThrowIfUserExists(string email)
    {
        var exists = await _appUserRepository.Exists(email);
        if (exists)
        {
            throw new ConflictException(ErrorCodesEnum.Exists);
        }
    }

    private SecurityToken GetRegistrationToken(int userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimExtension.UserIdClaimKey, userId.ToString()),
            new(ClaimExtension.RegistrationConfirmationClaim, true.ToString())
        };
        return _tokenService.GetToken(claims);
    }
}