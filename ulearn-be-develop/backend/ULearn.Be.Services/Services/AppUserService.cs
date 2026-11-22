using System.Security.Claims;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.AppUser.Sitemap;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Services.Utils;
using ULearn.Be.Services.Utils;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppUserService : IAppUserService
{
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppReservationSharedService _appReservationSharedService;
    private readonly IAppRoomActivityRepository _appRoomActivityRepository;
    private readonly IAppRoomRepository _appRoomRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserSharedService _appUserSharedService;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly ICTranslationLanguageRepository _cTranslationLanguageRepository;
    private readonly IHubspotSharedService _hubspotSharedService;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly IRUserFileRepository _rUserFileRepository;
    private readonly ITimeUtility _timeUtility;

    public AppUserService(
        IAppUserRepository appUserRepository,
        IRUserFileRepository rUserFileRepository,
        INotificationSharedService notificationSharedService,
        IAppReservationRepository appReservationRepository,
        IAppRoomRepository appRoomRepository,
        IAppRoomActivityRepository appRoomActivityRepository,
        ICTranslationLanguageRepository cTranslationLanguageRepository,
        IAppUserSharedService appUserSharedService,
        IAppUserWalletRepository appUserWalletRepository,
        IAppReservationSharedService appReservationSharedService,
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletSharedService appUserWalletSharedService,
        ITimeUtility timeUtility,
        IHubspotSharedService hubspotSharedService
    )
    {
        _appUserRepository = appUserRepository;
        _rUserFileRepository = rUserFileRepository;
        _notificationSharedService = notificationSharedService;
        _appReservationRepository = appReservationRepository;
        _appRoomRepository = appRoomRepository;
        _appRoomActivityRepository = appRoomActivityRepository;
        _cTranslationLanguageRepository = cTranslationLanguageRepository;
        _appUserSharedService = appUserSharedService;
        _appUserWalletRepository = appUserWalletRepository;
        _appReservationSharedService = appReservationSharedService;
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletSharedService = appUserWalletSharedService;
        _timeUtility = timeUtility;
        _hubspotSharedService = hubspotSharedService;
    }

    public async Task<ICollection<Claim>> Update(AppUserIM appUserIm, int userId, ClaimsPrincipal principal)
    {
        var appUser = await _appUserRepository.GetRawDbEntity(appUserIm.Login, false);
        ThrowIfNonAdminConfirmingTutor(appUserIm, principal, appUser);
        ThrowIfEmailBelongsToAnotherUser(userId, appUser.UserId);
        ThrowIfChangingRoleAndAlreadyConfirmed(appUserIm.UserRoleId, appUser.UserRoleId, appUser.UserStateId);
        await ThrowIfMissingRequiredFields(appUserIm, appUser);
        await _appUserRepository.Upsert(appUserIm, userId, inviterId: appUser.InviterId);
        await UpdateWallet(appUserIm, userId);
        await _hubspotSharedService.SyncUser(userId);
        await SendWaitingForApprovalNotificationIfRequired(
            userId,
            appUser.UserRoleId,
            previousStateId: appUser.UserStateId,
            newStateId: appUserIm.UserStateId
        );
        await SendRegistrationCompletedIfRequired(
            userId,
            appUser.UserRoleId,
            previousStateId: appUser.UserStateId,
            newStateId: appUserIm.UserStateId
        );
        return AuthUtil.GetClaims(await _appUserRepository.GetById(userId, onlyConfirmed: false, includeDeleted: true));
    }

    public Task<PagedVM<AppUserOverviewVM>> UserFilter(
        FilterView<UserFilterVM> filter,
        CUserRoleEnum userRoleEnum,
        ClaimsPrincipal user
    )
    {
        return _appUserSharedService.Filter(filter, userRoleEnum, user);
    }

    public async Task<AppShortUserInfoVM> GetShortInfo(ClaimsPrincipal user)
    {
        var currentUserId = user.GetUserId();
        var basicInfo = await _appUserRepository.GetById(currentUserId, onlyConfirmed: false);
        return new AppShortUserInfoVM(basicInfo)
        {
            RunningActivity = await GetRunningActivity(currentUserId),
            HasFutureBookings = await _appReservationRepository.ExistsFutureBookings(currentUserId),
            HasBookedAnyLesson = await _appReservationRepository.ExistsAnyBookings(currentUserId)
        };
    }

    public Task<PagedVM<AppRelatedUserVM>> GetStudents(ClaimsPrincipal user)
    {
        return _appUserSharedService.GetStudents(user);
    }

    public Task<PagedVM<AppRelatedUserVM>> GetTutors(ClaimsPrincipal user)
    {
        return _appUserSharedService.GetTutors(user);
    }

    public async Task<PagedVM<AppUserEmployeeVM>> EmployeesFilter(
        ClaimsPrincipal user,
        FilterView<AppUserEmployeeFilterVM> filter
    )
    {
        var results = await _appUserRepository.GetEmployeeFilter(user.GetUserId(), filter);
        await EnhanceWithEmployeesWalletInfo(results.Data);
        await EnhanceWithEmployeesLessonStatistics(results.Data);
        await EnhanceWithEmployeesCurrentWeeklyGoalsProgress(results.Data);
        return results;
    }

    public Task<AppTutorAdminInfoVM> GetTutorAdminInfo(int userId, ClaimsPrincipal user)
    {
        return _appUserRepository.GetTutorAdminInfo(userId);
    }

    public async Task Approve(int userId, ClaimsPrincipal user)
    {
        await _appUserRepository.Approve(userId);
        await _notificationSharedService.SendProfiledVerified(userId);
        await _hubspotSharedService.SyncUser(userId);
    }

    public async Task Refuse(int userId, ClaimsPrincipal user)
    {
        await _appUserRepository.Refuse(userId);
        await _hubspotSharedService.SyncUser(userId);
    }

    public async Task ChangeTranslationLanguage(string languageCode, ClaimsPrincipal user)
    {
        var translationLanguageId = await _cTranslationLanguageRepository.GetTranslationLanguageId(languageCode);
        await _appUserRepository.ChangeLanguage(translationLanguageId, user.GetUserId());
    }

    public async Task<AppUserBasicVM> UpdatePassword(PasswordUpdateIM input, ClaimsPrincipal user)
    {
        var userInfoBefore = await _appUserRepository.GetById(user.GetUserId(), onlyConfirmed: false);
        var isEmployee = userInfoBefore.InviterId.HasValue;
        await _appUserRepository.UpdatePassword(new PasswordUpdateIM
        {
            NewPassword = PasswordUtil.HashPassword(input.NewPassword ?? "")
        }, user.GetUserId());

        if (userInfoBefore.StateId == (int)CUserStateEnum.PasswordSetup)
        {
            var nextState = isEmployee ? CUserStateEnum.Registration : CUserStateEnum.RoleSetup;
            await _appUserRepository.UpdateState(user.GetUserId(), nextState);
        }

        return await _appUserRepository.GetById(user.GetUserId(), onlyConfirmed: false);
    }

    public async Task DeleteEmployeeAccount(AppUserDeleteEmployeeIM input, ClaimsPrincipal user)
    {
        var adminId = user.GetUserId();
        await ThrowIfInvalidEmployee(input.UserId, adminId);
        await CancelRemainingReservations(input.UserId);
        await TransferMoneyBack(input.UserId, adminId);
        await _appUserRepository.UpdateState(input.UserId, CUserStateEnum.Deleted);
        await _hubspotSharedService.SyncUser(input.UserId);
    }

    public async Task BatchHardAdminDelete(AppUserAdminBatchDeleteIM input, ClaimsPrincipal user)
    {
        foreach (var inputEmail in input.Emails)
        {
            await _appUserRepository.BatchHardDelete(new AppUserAdminBatchDeleteIM{ Emails = new List<string>{inputEmail}});
        }
    }

    public Task<List<AppUserSitemapVM>> GetTutorsSitemapInfo(ClaimsPrincipal user)
    {
        return _appUserRepository.GetTutorSitemapInfo();
    }

    public async Task UpdateEmployeeAccount(AppEmployeeIM input, ClaimsPrincipal user)
    {
        var adminId = user.GetUserId();
        await ThrowIfInvalidEmployee(input.UserId, adminId);
        await _appUserRepository.UpdateEmployeeAccount(input);
    }

    public Task AdminUpdate(AppUserAdminIM input, ClaimsPrincipal user)
    {
        return _appUserRepository.AdminUpdate(input);
    }

    public Task<AppUserPublicVM> GetTutorPublicProfile(int userId, ClaimsPrincipal principal)
    {
        return _appUserSharedService.GetTutorPublicProfile(userId, principal);
    }

    public Task<AppUserVM> GetPrivateProfile(ClaimsPrincipal principal)
    {
        return _appUserSharedService.GetPrivateProfile(principal);
    }

    private static void ThrowIfChangingRoleAndAlreadyConfirmed(int newRoleId, int previousRoleId, int userStateId)
    {
        if (newRoleId == previousRoleId)
        {
            return;
        }

        if (userStateId != (int)CUserStateEnum.Confirmed)
        {
            return;
        }

        throw new ForbiddenException(ErrorCodesEnum.CannotChangeRoleOnesAlreadyConfirmed);
    }

    private async Task CancelRemainingReservations(int userId)
    {
        var remainingReservations = await _appReservationRepository.GetRemainingReservationsByUserId(userId);
        var packageIds = remainingReservations.Select(x => x.ReservationPackageId).ToHashSet();
        foreach (var packageId in packageIds)
        {
            await _appReservationSharedService.CancelRemainingReservations(packageId, userId, "Account deleted",
                checkCancellationAmount: false);
        }
    }

    private async Task TransferMoneyBack(int userId, int adminId)
    {
        var now = _timeUtility.GetUtcNow();
        var amount = await _appTransactionRepository.GetCurrentBalance(userId);
        var userWalletId = await _appUserWalletRepository.GetUserWalletId(userId);
        var adminWalletId = await _appUserWalletRepository.GetUserWalletId(adminId);
        await _appTransactionRepository.Add(
            new AppTransactionGroupIM(
                new AppTransactionIM(
                    creatorId: adminId,
                    vendorId: adminId,
                    customerId: adminId,
                    ownerWalletId: adminWalletId,
                    transactionTypeEnum: CTransactionTypeEnum.MoneyTransfer,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    amount: amount,
                    plannedCompletionTime: now
                ),
                new AppTransactionIM(
                    creatorId: adminId,
                    vendorId: adminId,
                    customerId: adminId,
                    ownerWalletId: userWalletId,
                    transactionTypeEnum: CTransactionTypeEnum.MoneyTransfer,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    amount: -amount,
                    plannedCompletionTime: now
                )
            ));
        await _appUserWalletSharedService.UpdateWalletsStatistics(adminId, userId);
    }

    private async Task ThrowIfInvalidEmployee(int userId, int adminId)
    {
        var exists = await _appUserRepository.ExistsEmployee(adminId, userId);
        if (!exists)
        {
            throw new ConflictException(ErrorCodesEnum.InvalidEmployeeId);
        }
    }

    private async Task EnhanceWithEmployeesWalletInfo(ICollection<AppUserEmployeeVM> results)
    {
        var walletInfoByUserId = await _appUserWalletRepository
            .GetEmployeesWalletInfo(results.Select(x => x.UserId));

        foreach (var appUserEmployeeVm in results.Where(x => walletInfoByUserId.ContainsKey(x.UserId)))
        {
            appUserEmployeeVm.Wallet = walletInfoByUserId[appUserEmployeeVm.UserId];
        }
    }

    private async Task EnhanceWithEmployeesLessonStatistics(ICollection<AppUserEmployeeVM> results)
    {
        var lessonStatisticsByUserId = await _appReservationRepository.GetEmployeesLessonStatistics(
            results.Select(x => x.UserId)
        );
        foreach (var appUserEmployeeVm in results.Where(x => lessonStatisticsByUserId.ContainsKey(x.UserId)))
        {
            appUserEmployeeVm.LessonStatistics = lessonStatisticsByUserId[appUserEmployeeVm.UserId];
        }
    }

    private async Task EnhanceWithEmployeesCurrentWeeklyGoalsProgress(ICollection<AppUserEmployeeVM> results)
    {
        var lessonStatisticsByUserId = await _appReservationRepository.GetAttendersCurrentWeeklyGaolsProgress(
            results.Where(x => x.WeeklyGoalId.HasValue).Select(x => x.UserId)
        );
        foreach (var appUserEmployeeVm in results.Where(x => lessonStatisticsByUserId.ContainsKey(x.UserId)))
        {
            appUserEmployeeVm.CurrentWeeklyGoalProgress = UserStatisticUtil.GetCurrentWeeklyGoalProgress(
                lessonStatisticsByUserId[appUserEmployeeVm.UserId],
                appUserEmployeeVm.WeeklyGoalId
            );
        }
    }

    private async Task UpdateWallet(AppUserIM appUserIm, int userId)
    {
        var walletId = await _appUserWalletRepository.GetUserWalletId(userId);
        await _appUserWalletRepository.UpdateBankAccountInformation(
            walletId,
            appUserIm.BankAccountNumber,
            appUserIm.BankCode
        );
    }

    private async Task<AppReservationVM> GetRunningActivity(int currentUserId)
    {
        var activity = await _appReservationRepository.GetRunningActivity(currentUserId);
        if (activity == null)
        {
            return null;
        }

        var room = await _appRoomRepository.GetById(activity.RoomId);
        var opponentId = activity.GetOpponentId(currentUserId);
        var userDictionary = await _appUserRepository.GetUserDictionary(new List<int> { opponentId });
        var opponent = userDictionary.First().Value;
        var latestActivityTypeId = await _appRoomActivityRepository.GetLatestActivityTypeId(room.RoomId, opponentId);
        activity.Room = room;
        activity.IsOpponentJoined = latestActivityTypeId >= CRoomActivityTypeEnum.Start;
        activity.Opponent = opponent;
        return activity;
    }

    private async Task ThrowIfMissingRequiredFields(AppUserIM appUserIm, AppUser user)
    {
        if (appUserIm.UserStateId != (int)CUserStateEnum.ProfileValidation)
        {
            return;
        }

        if (user.UserRoleId == (int)CUserRoleEnum.Student)
        {
            return;
        }

        if (user.UserRoleId == (int)CUserRoleEnum.Company)
        {
            return;
        }

        var errorCodes = new List<ErrorCodesEnum>();
        if (string.IsNullOrEmpty(appUserIm.FirstName))
        {
            errorCodes.Add(ErrorCodesEnum.MissingFirstName);
        }

        if (string.IsNullOrEmpty(appUserIm.LastName))
        {
            errorCodes.Add(ErrorCodesEnum.MissingLastName);
        }

        if (string.IsNullOrEmpty(appUserIm.Email))
        {
            errorCodes.Add(ErrorCodesEnum.MissingEmail);
        }

        if (string.IsNullOrEmpty(appUserIm.Login))
        {
            errorCodes.Add(ErrorCodesEnum.MissingEmail);
        }

        if (appUserIm.CountryId == null)
        {
            errorCodes.Add(ErrorCodesEnum.MissingCountryOfOrigin);
        }

        if (appUserIm.Languages?.Any() is false)
        {
            errorCodes.Add(ErrorCodesEnum.MissingSpokenLanguage);
        }

        if (appUserIm.InterestIds?.Any() is false)
        {
            errorCodes.Add(ErrorCodesEnum.MissingInterests);
        }

        if (appUserIm.SubjectId == null)
        {
            errorCodes.Add(ErrorCodesEnum.MissingSubject);
        }

        if (appUserIm.SpecializationIds?.Any() is false)
        {
            errorCodes.Add(ErrorCodesEnum.MissingSpecializations);
        }

        if (appUserIm.SubjectLevelIds?.Any() is false)
        {
            errorCodes.Add(ErrorCodesEnum.MissingSubjectLevel);
        }

        if (string.IsNullOrEmpty(appUserIm.Phone))
        {
            errorCodes.Add(ErrorCodesEnum.MissingPhoneNumber);
        }

        var description = appUserIm.Attributes?.FirstOrDefault(x =>
            x.AttributeTypeId == (int)CUserAttributeTypeEnum.Description
        )?.Value;
        if (string.IsNullOrEmpty(description) || description.Length < 100)
        {
            errorCodes.Add(ErrorCodesEnum.DescriptionTooShort);
        }

        if (appUserIm.TeachingExperienceId == null)
        {
            errorCodes.Add(ErrorCodesEnum.MissingTeachingExperience);
        }

        if (!await _rUserFileRepository.Exists(user.UserId, CFileTypeEnum.ProfilePhoto))
        {
            errorCodes.Add(ErrorCodesEnum.MissingProfilePhoto);
        }

        if (appUserIm.HourRate == null)
        {
            errorCodes.Add(ErrorCodesEnum.MissingHourRate);
        }

        if (appUserIm.HourRate <= 0)
        {
            errorCodes.Add(ErrorCodesEnum.InvalidHourRate);
        }

        if (string.IsNullOrEmpty(appUserIm.Ico))
        {
            errorCodes.Add(ErrorCodesEnum.MissingIco);
        }

        if (string.IsNullOrEmpty(appUserIm.PlaceOfBusiness))
        {
            errorCodes.Add(ErrorCodesEnum.MissingPlaceOfBusiness);
        }

        if (!errorCodes.Any())
        {
            return;
        }

        throw new ConflictException(
            ErrorCodesEnum.SomeFieldsAreEmpty,
            errorCodes.ToDictionary(x => x.GetDescription(), _ => "")
        );
    }

    private static void ThrowIfNonAdminConfirmingTutor(
        AppUserIM appUserIm,
        ClaimsPrincipal principal,
        AppUser appUser
    )
    {
        if (appUserIm.UserStateId != (int)CUserStateEnum.Confirmed)
        {
            return;
        }

        if (appUserIm.UserStateId == appUser.UserStateId)
        {
            return;
        }

        if (appUser.UserRoleId != (int)CUserRoleEnum.Tutor)
        {
            return;
        }

        if (principal.IsAdmin())
        {
            return;
        }

        throw new ForbiddenException();
    }

    private void ThrowIfEmailBelongsToAnotherUser(int currentUserId, int? appUserIdWithNewTheMail)
    {
        if (appUserIdWithNewTheMail == null)
        {
            return;
        }

        if (appUserIdWithNewTheMail == currentUserId)
        {
            return;
        }

        throw new ConflictException(ErrorCodesEnum.EmailExists);
    }

    private async Task SendWaitingForApprovalNotificationIfRequired(
        int userId,
        int userRoleId,
        int previousStateId,
        int newStateId
    )
    {
        if (userRoleId != (int)CUserRoleEnum.Tutor)
        {
            return;
        }

        if (previousStateId == newStateId)
        {
            return;
        }

        if (newStateId != (int)CUserStateEnum.ProfileValidation)
        {
            return;
        }

        await _notificationSharedService.SendNewTutorWaitingForApproval(userId);
    }

    private async Task SendRegistrationCompletedIfRequired(
        int userId,
        int userRoleId,
        int previousStateId,
        int newStateId
    )
    {
        if (userRoleId == (int)CUserRoleEnum.Tutor)
        {
            return;
        }

        if (previousStateId == newStateId)
        {
            return;
        }

        if (newStateId != (int)CUserStateEnum.Confirmed)
        {
            return;
        }

        await _notificationSharedService.SendRegistrationCompleted(userId);
    }
}