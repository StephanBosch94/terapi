using Quartz;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace Yaspeak.Be.Scheduler.Jobs;

public class AppTransactionScheduledJob : IJob
{
    public static readonly JobKey Key = new("app-transaction-job");
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly ILogger<AppTransactionScheduledJob> _logger;
    private readonly ITimeUtility _timeUtility;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppTransactionScheduledJob(
        ILogger<AppTransactionScheduledJob> logger,
        IAppTransactionRepository appTransactionRepository,
        IAppReservationRepository appReservationRepository,
        ITimeUtility timeUtility,
        IAppUserWalletRepository appUserWalletRepository,
        IDbContextTransactionHandler transactionHandler,
        IAppUserWalletSharedService appUserWalletSharedService
    )
    {
        _logger = logger;
        _appTransactionRepository = appTransactionRepository;
        _appReservationRepository = appReservationRepository;
        _timeUtility = timeUtility;
        _appUserWalletRepository = appUserWalletRepository;
        _transactionHandler = transactionHandler;
        _appUserWalletSharedService = appUserWalletSharedService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _transactionHandler.WithTransaction(() => Run(context));
    }

    private async Task Run(IJobExecutionContext context)
    {
        await CheckAndUpdateReservationsStates();
        await CheckAndUpdateCompletedTransactions();
    }

    private async Task CheckAndUpdateReservationsStates()
    {
        _logger.LogInformation("Checking pastReadyForPaymentScheduleReservations");
        var pastReadyForPaymentScheduleReservations =
            await _appReservationRepository.GetPastReadyForPaymentScheduleReservations();
        await _appTransactionRepository.UpdateStateByReservationId(
            pastReadyForPaymentScheduleReservations.Select(x => x.ReservationId),
            CTransactionStateEnum.Scheduled,
            CommonConstants.TechnicalAdminUserId
        );
        await _appReservationRepository.UpdateState(
            pastReadyForPaymentScheduleReservations.Select(x => x.ReservationId),
            CReservationStateEnum.ScheduledForPayment
        );
    }

    private async Task CheckAndUpdateCompletedTransactions()
    {
        _logger.LogInformation("Checking pastReadyForPaymentReservations");
        var pastReadyForPaymentReservations = await _appReservationRepository.GetPastReadyForPaymentReservations();
        var reservationIds = pastReadyForPaymentReservations.Select(x => x.ReservationId).ToHashSet();
        var tutorIds = pastReadyForPaymentReservations.Select(x => x.HostId).ToHashSet();

        var updatedTransactionsCount = await _appTransactionRepository.UpdateStateByReservationId(reservationIds,
            CTransactionStateEnum.Completed, CommonConstants.TechnicalAdminUserId);
        await _appReservationRepository.UpdateState(
            reservationIds,
            CReservationStateEnum.Payed
        );
        if (updatedTransactionsCount > 0)
        {
            await _appUserWalletSharedService.UpdateWalletsStatistics(
                tutorIds
                    .Concat(new List<int> { CommonConstants.TechnicalAdminUserId }).ToHashSet());
        }

        _logger.LogInformation(
            "Completed [{TransactionsCount}] transactions in [{ReservationsCount}] reservations",
            updatedTransactionsCount,
            reservationIds.Count
        );
    }
}