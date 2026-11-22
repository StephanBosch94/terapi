using System.Security.Claims;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppTransactionService : IAppTransactionService
{
    private readonly IAppReservationPackageRepository _appReservationPackageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly InstanceSettings _instanceSettings;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly ITransactionExportService _transactionExportService;

    public AppTransactionService(
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletRepository appUserWalletRepository,
        IAppUserWalletSharedService appUserWalletSharedService,
        IAppReservationRepository appReservationRepository,
        IAppReservationPackageRepository appReservationPackageRepository,
        INotificationSharedService notificationSharedService,
        IAppUserRepository appUserRepository,
        ITransactionExportService transactionExportService,
        IEnvConfigurationProvider envConfigurationProvider
    )
    {
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletRepository = appUserWalletRepository;
        _appUserWalletSharedService = appUserWalletSharedService;
        _appReservationRepository = appReservationRepository;
        _appReservationPackageRepository = appReservationPackageRepository;
        _notificationSharedService = notificationSharedService;
        _appUserRepository = appUserRepository;
        _transactionExportService = transactionExportService;
        _instanceSettings = envConfigurationProvider.GetSettings<InstanceSettings>(EnvSettingsKeyEnum.InstanceSettings);
    }

    public async Task<PagedVM<AppTransactionVM>> Filter(FilterView<AppTransactionFilterVM> filter, ClaimsPrincipal user)
    {
        var result = await _appTransactionRepository.Filter(filter, user.GetUserId());

        await EnhanceWithOpponents(result.Data, user);
        await EnhanceWithReservations(result.Data);
        EnhanceWithUrls(result.Data);

        return result;
    }

    public Task<PagedVM<AppTransactionWithdrawVM>> WithdrawFilter(FilterView<AppTransactionWithdrawFilterVM> filter)
    {
        return _appTransactionRepository.WithdrawFilter(filter);
    }

    public async Task CompleteTransaction(int transactionId, ClaimsPrincipal user)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var creatorRoleEnum = await _appUserRepository.GetRoleId(transaction.CreatorId);
        var wallet = await _appUserWalletRepository.GetRawDbEntity(transaction.UserWalletId);
        await _appTransactionRepository.Complete(transactionId, user.GetUserId());
        await _appUserWalletSharedService.UpdateWalletsStatistics(wallet.UserId);

        if (creatorRoleEnum == CUserRoleEnum.Student)
        {
            await _appTransactionRepository.Complete(transaction.SourceTransactionId!.Value, user.GetUserId());
            await _appUserWalletSharedService.UpdateWalletsStatistics(CommonConstants.TechnicalAdminUserId);
        }

        await _notificationSharedService.SendMoneySent(transactionId);
    }

    public Task<AppTransactionPdfFileVM> GetFile(int transactionId, ClaimsPrincipal user)
    {
        return _transactionExportService.ExportToPdf(transactionId, user);
    }

    public Task<AppTransactionCsvFileVM> Export(AppTransactionExportIM options, ClaimsPrincipal user)
    {
        return _transactionExportService.ExportAllToCsv(options, user);
    }

    private async Task EnhanceWithOpponents(ICollection<AppTransactionVM> results, ClaimsPrincipal principal)
    {
        var userId = principal.GetUserId();
        var reservationIds = results.Where(x => x.ReservationId != null).Select(x => x.ReservationId.Value).ToHashSet();
        var reservationPackageIds = results.Where(x => x.ReservationPackageId != null)
            .Select(x => x.ReservationPackageId.Value).ToHashSet();
        var reservationOpponentsByReservationId =
            await _appReservationRepository.GetOpponentByReservationId(reservationIds, userId);
        var reservationPackageOpponentsByPackageId =
            await _appReservationPackageRepository.GetOpponentByPackageId(reservationPackageIds, userId);
        foreach (var appTransactionVm in results)
        {
            var reservationOpponent = appTransactionVm.ReservationId.HasValue &&
                                      reservationOpponentsByReservationId.ContainsKey(appTransactionVm.ReservationId
                                          .Value)
                ? reservationOpponentsByReservationId[appTransactionVm.ReservationId.Value]
                : null;
            var reservationPackageOpponent = appTransactionVm.ReservationPackageId.HasValue &&
                                             reservationPackageOpponentsByPackageId.ContainsKey(appTransactionVm
                                                 .ReservationPackageId.Value)
                ? reservationPackageOpponentsByPackageId[appTransactionVm.ReservationPackageId.Value]
                : null;
            appTransactionVm.Opponent = reservationOpponent ?? reservationPackageOpponent;
        }
    }

    private async Task EnhanceWithReservations(ICollection<AppTransactionVM> resultData)
    {
        var reservationByReservationId = await _appReservationRepository.GetTransactionReservations(
            resultData.Where(x => x.ReservationId != null)
                .Select(x => x.ReservationId.Value)
                .ToHashSet()
        );

        foreach (var appTransactionVm in resultData.Where(x => x.ReservationId != null))
        {
            appTransactionVm.Reservation = reservationByReservationId[appTransactionVm.ReservationId!.Value];
        }
    }

    private void EnhanceWithUrls(IEnumerable<AppTransactionVM> resultData)
    {
        var urlBase = _instanceSettings.BeApiUrl.LastOrDefault() == '/'
            ? _instanceSettings.BeApiUrl
            : _instanceSettings.BeApiUrl + "/";
        foreach (var appTransactionVm in resultData.Where(x => string.IsNullOrEmpty(x.ReceiptUrl)))
        {
            appTransactionVm.ReceiptUrl = $"{urlBase}api/transaction/{appTransactionVm.TransactionId}/export";
        }
    }
}