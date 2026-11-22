using System.Security.Claims;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppReservationIssue;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppReservationIssueService : IAppReservationIssueService
{
    private readonly IAppReservationIssueRepository _appReservationIssueRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly ITimeUtility _timeUtility;

    public AppReservationIssueService(
        IAppReservationIssueRepository appReservationIssueRepository,
        IAppReservationRepository appReservationRepository,
        IAppUserWalletRepository appUserWalletRepository,
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletSharedService appUserWalletSharedService,
        INotificationSharedService notificationSharedService,
        IAppUserRepository appUserRepository,
        ITimeUtility timeUtility
    )
    {
        _appReservationIssueRepository = appReservationIssueRepository;
        _appReservationRepository = appReservationRepository;
        _appUserWalletRepository = appUserWalletRepository;
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletSharedService = appUserWalletSharedService;
        _notificationSharedService = notificationSharedService;
        _appUserRepository = appUserRepository;
        _timeUtility = timeUtility;
    }

    public async Task Add(ReservationIssueIM input, ClaimsPrincipal user)
    {
        var reservation = await _appReservationRepository.GetRawDbEntity(input.ReservationId);

        await _appReservationRepository.UpdateState(
            new List<int> { input.ReservationId },
            CReservationStateEnum.WaitingForReview
        );

        await _appTransactionRepository.UpdateStateByReservationId(
            input.ReservationId,
            CTransactionStateEnum.WaitingForReview,
            user.GetUserId()
        );

        await _appUserWalletSharedService.UpdateWalletsStatistics(
            new List<int>
            {
                reservation.HostId,
                CommonConstants.TechnicalAdminUserId
            });

        var userId = user.IsAdmin()
            ? reservation.AttenderId
            : user.GetUserId();

        var reservationIssueId = await _appReservationIssueRepository.Add(input, userId);
        await _notificationSharedService.SendNewReservationIssue(reservationIssueId);
    }

    public async Task ApproveRefund(ReservationIssueApproveIM input, ClaimsPrincipal user)
    {
        ThrowIfInvalidRefundAmount(input);

        var reservationIssue = await _appReservationIssueRepository.GetRawDbEntity(input.ReservationIssueId);
        var reservation = await _appReservationRepository.GetRawDbEntity(reservationIssue.ReservationId);
        await _appReservationIssueRepository.Resolve(
            input.ReservationIssueId,
            CReservationIssueStateEnum.Approved,
            input.DecisionText,
            user.GetUserId()
        );
        await _appTransactionRepository.UpdateStateByReservationId(
            reservation.ReservationId,
            CTransactionStateEnum.Canceled,
            CommonConstants.TechnicalAdminUserId,
            new List<CTransactionTypeEnum>
            {
                CTransactionTypeEnum.LessonCommission,
                CTransactionTypeEnum.LessonPayment
            }
        );

        await _appReservationRepository.UpdateState(
            new List<int> { reservation.ReservationId },
            CReservationStateEnum.Refunded
        );

        var transactions = await GetRefundTransactions(reservationIssue, reservation, input, user);
        await _appTransactionRepository.AddRange(transactions);

        await _appUserWalletSharedService.UpdateWalletsStatistics(
            new List<int> { CommonConstants.TechnicalAdminUserId, reservation.HostId, reservation.AttenderId }
        );
        await _notificationSharedService.SendRefundApproved(input.ReservationIssueId);
    }

    public async Task DeclineRefund(ReservationIssueDecisionIM input, ClaimsPrincipal user)
    {
        var reservationIssue = await _appReservationIssueRepository.GetRawDbEntity(input.ReservationIssueId);
        var reservation = await _appReservationRepository.GetRawDbEntity(reservationIssue.ReservationId);

        await _appReservationIssueRepository.Resolve(
            input.ReservationIssueId,
            CReservationIssueStateEnum.Declined,
            input.DecisionText,
            user.GetUserId()
        );

        await _appTransactionRepository.UpdateStateByReservationId(
            reservation.ReservationId,
            CTransactionStateEnum.Completed,
            CommonConstants.TechnicalAdminUserId,
            new List<CTransactionTypeEnum>
            {
                CTransactionTypeEnum.LessonCommission,
                CTransactionTypeEnum.LessonPayment
            }
        );

        await _appReservationRepository.UpdateState(
            new List<int> { reservationIssue.ReservationId },
            CReservationStateEnum.Payed
        );

        await _appUserWalletSharedService.UpdateWalletsStatistics(
            new List<int>
            {
                reservation.HostId,
                CommonConstants.TechnicalAdminUserId
            }
        );
        await _notificationSharedService.SendRefundDeclined(input.ReservationIssueId);
    }

    public Task<PagedVM<AppReservationIssueVM>> Filter(FilterView<AppReservationIssueFilterVM> filter,
        ClaimsPrincipal user)
    {
        return _appReservationIssueRepository.Filter(filter);
    }

    private async Task<IReadOnlyCollection<AppTransactionGroupIM>> GetRefundTransactions(
        AppReservationIssue reservationIssue,
        AppReservation reservation,
        ReservationIssueApproveIM input,
        ClaimsPrincipal principal
    )
    {
        var currentUserId = principal.GetUserId();
        var transactions = await _appTransactionRepository.GetRawDbEntitiesByReservationId(reservation.ReservationId);

        return new List<AppTransactionGroupIM>
            {
                await GetStudentCreditMemoTransactions(reservationIssue, reservation, transactions, input,
                    currentUserId),
                await GetTutorCommissionCreditMemoTransactions(reservationIssue, reservation, transactions, input,
                    currentUserId)
            }
            .Where(x => x != null)
            .ToList();
    }

    private async Task<AppTransactionGroupIM> GetStudentCreditMemoTransactions(
        AppReservationIssue reservationIssue,
        AppReservation reservation,
        IReadOnlyCollection<AppTransaction> transactions,
        ReservationIssueApproveIM input,
        int currentUserId
    )
    {
        var studentReturnAmount = input.StudentPercentage * reservation.Price;
        if (studentReturnAmount <= 0)
        {
            return null;
        }

        var now = _timeUtility.GetUtcNow();
        var student = await _appUserRepository.GetRawDbEntity(reservation.AttenderId);
        var customerId = student.InviterId ?? student.UserId;
        var studentWalletId = await _appUserWalletRepository.GetUserWalletId(reservation.AttenderId);
        var tutorWalletId = await _appUserWalletRepository.GetUserWalletId(reservation.HostId);

        return new AppTransactionGroupIM(
            new AppTransactionIM
            (
                ownerWalletId: studentWalletId,
                vendorId: reservation.HostId,
                customerId: customerId,
                reservationIssueId: reservationIssue.ReservationIssueId,
                reservationId: reservation.ReservationId,
                reservationPackageId: reservation.ReservationPackageId,
                amount: studentReturnAmount,
                transactionTypeEnum: CTransactionTypeEnum.RefundCreditMemo,
                transactionStateEnum: CTransactionStateEnum.Completed,
                transactionRefundReasonEnum: CTransactionRefundReasonEnum.Refund,
                creatorId: currentUserId,
                plannedCompletionTime: now,
                sourceTransactionId: transactions
                    .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonPayment)
                    .Where(x => x.UserWalletId == studentWalletId)
                    .Select(x => (int?)x.TransactionId)
                    .FirstOrDefault()
            ),
            new AppTransactionIM
            (
                ownerWalletId: tutorWalletId,
                vendorId: reservation.HostId,
                customerId: customerId,
                reservationIssueId: reservationIssue.ReservationIssueId,
                reservationId: reservation.ReservationId,
                reservationPackageId: reservation.ReservationPackageId,
                amount: -studentReturnAmount,
                transactionTypeEnum: CTransactionTypeEnum.RefundCreditMemo,
                transactionStateEnum: CTransactionStateEnum.Completed,
                transactionRefundReasonEnum: CTransactionRefundReasonEnum.Refund,
                creatorId: currentUserId,
                plannedCompletionTime: now,
                sourceTransactionId: transactions
                    .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonPayment)
                    .Where(x => x.UserWalletId == tutorWalletId)
                    .Select(x => (int?)x.TransactionId)
                    .FirstOrDefault()
            )
        );
    }

    private async Task<AppTransactionGroupIM> GetTutorCommissionCreditMemoTransactions(
        AppReservationIssue reservationIssue,
        AppReservation reservation,
        IReadOnlyCollection<AppTransaction> transactions,
        ReservationIssueApproveIM input,
        int currentUserId
    )
    {
        var now = _timeUtility.GetUtcNow();
        var lessonCommissionTransaction = await _appTransactionRepository.GetRawDbEntityByReservationIdIdAndType(
            reservationIssue.ReservationId,
            CTransactionTypeEnum.LessonCommission
        );
        var commissionReturnAmount =
            PriceUtil.GetRoundedPrice(input.StudentPercentage * lessonCommissionTransaction.Amount);
        var tutorWalletId = await _appUserWalletRepository.GetUserWalletId(reservation.HostId);
        var technicalAdminWalletId =
            await _appUserWalletRepository.GetUserWalletId(CommonConstants.TechnicalAdminUserId);
        return new AppTransactionGroupIM
        (
            new AppTransactionIM
            (
                ownerWalletId: tutorWalletId,
                vendorId: CommonConstants.TechnicalAdminUserId,
                customerId: reservation.HostId,
                reservationIssueId: reservationIssue.ReservationIssueId,
                reservationId: reservation.ReservationId,
                reservationPackageId: reservation.ReservationPackageId,
                creatorId: currentUserId,
                amount: commissionReturnAmount,
                sessionId: null,
                transactionTypeEnum: CTransactionTypeEnum.RefundLessonCommissionCreditMemo,
                transactionStateEnum: CTransactionStateEnum.Completed,
                transactionRefundReasonEnum: CTransactionRefundReasonEnum.Refund,
                plannedCompletionTime: now,
                sourceTransactionId: transactions
                    .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonCommission)
                    .Where(x => x.UserWalletId == tutorWalletId)
                    .Select(x => (int?)x.TransactionId)
                    .FirstOrDefault()
            ),
            new AppTransactionIM
            (
                ownerWalletId: technicalAdminWalletId,
                vendorId: CommonConstants.TechnicalAdminUserId,
                customerId: reservation.HostId,
                reservationIssueId: reservationIssue.ReservationIssueId,
                reservationId: reservation.ReservationId,
                reservationPackageId: reservation.ReservationPackageId,
                creatorId: currentUserId,
                amount: -commissionReturnAmount,
                sessionId: null,
                transactionTypeEnum: CTransactionTypeEnum.RefundLessonCommissionCreditMemo,
                transactionStateEnum: CTransactionStateEnum.Completed,
                transactionRefundReasonEnum: CTransactionRefundReasonEnum.Refund,
                plannedCompletionTime: now,
                sourceTransactionId: transactions
                    .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonCommission)
                    .Where(x => x.UserWalletId == technicalAdminWalletId)
                    .Select(x => (int?)x.TransactionId)
                    .FirstOrDefault()
            )
        );
    }

    private static void ThrowIfInvalidRefundAmount(ReservationIssueApproveIM input)
    {
        if (input.StudentPercentage is <= 0 or > 1)
        {
            throw new ConflictException(ErrorCodesEnum.InvalidRefundPercentage);
        }
    }
}