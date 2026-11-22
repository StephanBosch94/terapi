using System.Security.Claims;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers.QueryMappers;
using ULearn.Be.Models.ViewModels.AppTimeslot;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class AppReservationSharedService : IAppReservationSharedService
{
    private readonly IAppReservationPackageRepository _appReservationPackageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppRoomRepository _appRoomRepository;
    private readonly IAppTimeslotRepository _appTimeslotRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserReservationReviewRepository _appUserReservationReviewRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly ITimeUtility _timeUtility;

    public AppReservationSharedService(
        IAppRoomRepository appRoomRepository,
        IAppUserRepository appUserRepository,
        ITimeUtility timeUtility,
        IAppReservationRepository appReservationRepository,
        IAppUserReservationReviewRepository appUserReservationReviewRepository,
        IAppTimeslotRepository appTimeslotRepository,
        IAppReservationPackageRepository appReservationPackageRepository,
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletRepository appUserWalletRepository,
        IAppUserWalletSharedService appUserWalletSharedService,
        INotificationSharedService notificationSharedService
    )
    {
        _appRoomRepository = appRoomRepository;
        _appUserRepository = appUserRepository;
        _timeUtility = timeUtility;
        _appReservationRepository = appReservationRepository;
        _appUserReservationReviewRepository = appUserReservationReviewRepository;
        _appTimeslotRepository = appTimeslotRepository;
        _appReservationPackageRepository = appReservationPackageRepository;
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletRepository = appUserWalletRepository;
        _appUserWalletSharedService = appUserWalletSharedService;
        _notificationSharedService = notificationSharedService;
    }

    public async Task<PagedVM<AppReservationVM>> Filter(
        FilterView<AppReservationFilter> filter,
        ClaimsPrincipal user
    )
    {
        var userId = user.GetUserId();
        var result = await _appReservationRepository.Filter(filter, userId);
        await Enhance(result.Data.ToList(), user);
        return result;
    }

    public async Task<Dictionary<int, AppReservationVM>> GetAll(IEnumerable<int> reservationIds, ClaimsPrincipal user)
    {
        var results = await _appReservationRepository.GetAll(reservationIds);
        await Enhance(results, user);
        return results.ToDictionary(x => x.ReservationId);
    }

    public async Task CreateTimeslot(int reservationId, int userId)
    {
        var reservation = await _appReservationRepository.GetRawDbEntity(reservationId);
        ThrowIfCannotEdit(reservation, userId);
        var startTimeConverted =
            _timeUtility.ConvertFromUtcToLocalTime(reservation.HostEventIanaTimezone, reservation.StartTime);
        var endTimeConverted =
            _timeUtility.ConvertFromUtcToLocalTime(reservation.HostEventIanaTimezone, reservation.EndTime);
        await _appTimeslotRepository.Add(new AppTimeslotInputVM
            {
                IanaTimezone = reservation.HostEventIanaTimezone,
                FromLocalYear = startTimeConverted.Year,
                FromLocalMonth = startTimeConverted.Month,
                FromLocalDay = startTimeConverted.Day,
                FromLocalHour = startTimeConverted.Hour,
                FromLocalMinute = startTimeConverted.Minute,
                DurationInMinutes = (int)(endTimeConverted - startTimeConverted).TotalMinutes
            },
            reservation.HostId
        );
    }

    public async Task CancelRemainingReservations(
        int reservationPackageId,
        int userId,
        string reasonText,
        bool checkCancellationAmount = true
    )
    {
        var remainingReservations =
            await _appReservationRepository.GetRemainingReservationsByPackageId(reservationPackageId);
        var remainingReservationIds = remainingReservations.Select(x => x.ReservationId).ToHashSet();
        var completedReservations =
            await _appReservationRepository.GetCompletedReservationsByPackageId(reservationPackageId);
        var package = await _appReservationPackageRepository.GetRawDbEntity(reservationPackageId);
        if (checkCancellationAmount)
        {
            ThrowIfInsufficientCancellationAmount(package.TotalPrice, completedReservations);
        }

        var tutorId = package.HostId;
        foreach (var remainingReservation in remainingReservations)
        {
            await CreateTimeslot(remainingReservation.ReservationId, userId);
        }

        await _appReservationPackageRepository.Cancel(reservationPackageId, reasonText);
        await _appReservationRepository.UpdateState(
            remainingReservationIds,
            CReservationStateEnum.CanceledByUser
        );
        await _appTransactionRepository.UpdateStateByReservationId(
            remainingReservations.Select(x => x.ReservationId),
            CTransactionStateEnum.Canceled,
            CommonConstants.TechnicalAdminUserId,
            new List<CTransactionTypeEnum> { CTransactionTypeEnum.LessonPayment, CTransactionTypeEnum.LessonCommission }
        );

        await CreateCancellationTransactions(
            package.AttenderId,
            package.HostId,
            package.ReservationPackageId,
            completedReservations,
            remainingReservations
        );
        await _notificationSharedService.SendPackageCancelled(reservationPackageId, senderId: userId,
            recipientId: tutorId);
    }

    public bool CanReschedule(ScheduledEntity entity)
    {
        var now = _timeUtility.GetUtcNow();
        if (entity.StartTime <= now)
        {
            return false;
        }

        return (entity.StartTime - now).TotalHours >= CommonConstants.MinimumTimeForClassRescheduleInHours;
    }

    private bool CanCancel(ScheduledEntity entity)
    {
        var now = _timeUtility.GetUtcNow();
        if (entity.StartTime <= now)
        {
            return false;
        }

        return (now - entity.StartTime).TotalHours < CommonConstants.MinimumTimeForClassCancellationForStudentInHours;
    }

    private static void ThrowIfInsufficientCancellationAmount(
        double totalPrice,
        IEnumerable<AppReservation> completedReservations
    )
    {
        var studentAmountToReturn = totalPrice - completedReservations.Sum(x => x.PriceWithoutSales);
        if (studentAmountToReturn <= 0)
        {
            throw new ConflictException(ErrorCodesEnum.InsufficientCancellationAmount);
        }
    }

    private async Task CreateCancellationTransactions(
        int studentId,
        int tutorId,
        int packageId,
        IReadOnlyCollection<AppReservation> completedReservations,
        IReadOnlyCollection<AppReservation> remainingReservations
    )
    {
        var now = _timeUtility.GetUtcNow();
        var student = await _appUserRepository.GetById(studentId);
        var attenderCustomerId = student.InviterId ?? student.UserId;
        var salesDiffAmount = completedReservations.Sum(x => x.PriceWithoutSales - x.Price - x.CouponSaleAmount ?? 0);
        var amountToReturn = remainingReservations.Sum(x => x.Price) - salesDiffAmount;
        var couponCancellationAmount = completedReservations
            .Where(x => x.CouponSaleAmount.HasValue)
            .Sum(x => x.CouponSaleAmount!.Value);
        var remainingLessonCommissions = PriceUtil.GetOurPriceCut(remainingReservations.Sum(x => x.Price), couponCancellationAmount);
        var salesCancellationPlatformFee = PriceUtil.GetOurPriceCut(salesDiffAmount, null);
        var studentWalletId = await _appUserWalletRepository.GetUserWalletId(studentId);
        var tutorWalletId = await _appUserWalletRepository.GetUserWalletId(tutorId);
        var technicalWalletId = await _appUserWalletRepository.GetUserWalletId(CommonConstants.TechnicalAdminUserId);
        await _appTransactionRepository.AddRange(new List<AppTransactionGroupIM?>
            {
                new(
                    new AppTransactionIM(
                        ownerWalletId: studentWalletId,
                        vendorId: tutorId,
                        customerId: attenderCustomerId,
                        amount: amountToReturn,
                        reservationPackageId: packageId,
                        reservationIssueId: null,
                        transactionTypeEnum: CTransactionTypeEnum.PackageCancellationCreditMemo,
                        transactionStateEnum: CTransactionStateEnum.Completed,
                        creatorId: CommonConstants.TechnicalAdminUserId,
                        transactionRefundReasonEnum: CTransactionRefundReasonEnum.PackageCancellation,
                        plannedCompletionTime: now
                    ),
                    new AppTransactionIM(
                        ownerWalletId: tutorWalletId,
                        vendorId: tutorId,
                        customerId: attenderCustomerId,
                        amount: -amountToReturn,
                        reservationPackageId: packageId,
                        reservationIssueId: null,
                        transactionTypeEnum: CTransactionTypeEnum.PackageCancellationCreditMemo,
                        transactionStateEnum: CTransactionStateEnum.Completed,
                        creatorId: CommonConstants.TechnicalAdminUserId,
                        transactionRefundReasonEnum: CTransactionRefundReasonEnum.PackageCancellation,
                        plannedCompletionTime: now
                    )
                ),
                new(
                    new AppTransactionIM
                    (
                        ownerWalletId: tutorWalletId,
                        vendorId: CommonConstants.TechnicalAdminUserId,
                        customerId: tutorId,
                        amount: remainingLessonCommissions,
                        reservationPackageId: packageId,
                        reservationIssueId: null,
                        transactionTypeEnum: CTransactionTypeEnum.RefundLessonCommissionCreditMemo,
                        transactionStateEnum: CTransactionStateEnum.Completed,
                        creatorId: CommonConstants.TechnicalAdminUserId,
                        transactionRefundReasonEnum: CTransactionRefundReasonEnum.PackageCancellation,
                        plannedCompletionTime: now
                    ),
                    new AppTransactionIM
                    (
                        ownerWalletId: technicalWalletId,
                        vendorId: CommonConstants.TechnicalAdminUserId,
                        customerId: tutorId,
                        amount: -remainingLessonCommissions,
                        reservationPackageId: packageId,
                        reservationIssueId: null,
                        transactionTypeEnum: CTransactionTypeEnum.RefundLessonCommissionCreditMemo,
                        transactionStateEnum: CTransactionStateEnum.Completed,
                        creatorId: CommonConstants.TechnicalAdminUserId,
                        transactionRefundReasonEnum: CTransactionRefundReasonEnum.PackageCancellation,
                        plannedCompletionTime: now
                    )
                ),
                completedReservations.Any()
                    ? new AppTransactionGroupIM(
                        new AppTransactionIM
                        (
                            ownerWalletId: tutorWalletId,
                            vendorId: CommonConstants.TechnicalAdminUserId,
                            customerId: tutorId,
                            amount: -salesCancellationPlatformFee,
                            reservationPackageId: packageId,
                            reservationIssueId: null,
                            transactionTypeEnum: CTransactionTypeEnum.PackageSaleCancellationCommission,
                            transactionStateEnum: CTransactionStateEnum.Completed,
                            creatorId: CommonConstants.TechnicalAdminUserId,
                            plannedCompletionTime: now
                        ),
                        new AppTransactionIM
                        (
                            ownerWalletId: technicalWalletId,
                            vendorId: CommonConstants.TechnicalAdminUserId,
                            customerId: tutorId,
                            amount: salesCancellationPlatformFee,
                            reservationPackageId: packageId,
                            reservationIssueId: null,
                            transactionTypeEnum: CTransactionTypeEnum.PackageSaleCancellationCommission,
                            transactionStateEnum: CTransactionStateEnum.Completed,
                            creatorId: CommonConstants.TechnicalAdminUserId,
                            plannedCompletionTime: now
                        )
                    )
                    : null
            }.Where(x => x != null)
        );

        await _appUserWalletSharedService.UpdateWalletsStatistics(
            tutorId, studentId, CommonConstants.TechnicalAdminUserId
        );
    }

    private static void ThrowIfCannotEdit(AppReservation reservation, int userId)
    {
        if (reservation.HostId != userId && reservation.AttenderId != userId)
        {
            throw new ForbiddenException();
        }
    }

    private async Task Enhance(IReadOnlyCollection<AppReservationVM> entities, ClaimsPrincipal user)
    {
        await EnhanceWithOpponents(entities, user.GetUserId());
        await EnhanceWithRooms(entities);
        await EnhanceWithReviews(entities);
        await EnhanceWithPackageInfo(entities);
        EnhanceWithCanCancel(entities);
        EnhanceWithCanReschedule(entities);
    }

    private async Task EnhanceWithPackageInfo(IReadOnlyCollection<AppReservationVM> entities)
    {
        var packageIds = entities.Select(x => x.ReservationPackageId).ToHashSet();
        var numberOfReservationsByPackageId =
            await _appReservationRepository.GetNumberOfReservationsByPackageId(packageIds);
        var numberOfUpcomingReservationsByPackageId =
            await _appReservationRepository.GetNumberOfRemainingLessonsByPackageId(packageIds);
        var rowNumberByReservationId = await _appReservationRepository.GetOrderNumberByReservationId(packageIds);

        foreach (var appReservationVm in entities)
        {
            numberOfReservationsByPackageId.TryGetValue(appReservationVm.ReservationPackageId, out var totalInPackage);
            rowNumberByReservationId.TryGetValue(appReservationVm.ReservationId, out var orderNumber);
            numberOfUpcomingReservationsByPackageId.TryGetValue(
                appReservationVm.ReservationPackageId,
                out var remainingInPackage
            );

            appReservationVm.TotalInPackage = totalInPackage;
            appReservationVm.OrderNumber = orderNumber;
            appReservationVm.RemainingInPackage = remainingInPackage;
            appReservationVm.RefundPrice = Math.Round(appReservationVm.Price * remainingInPackage,
                CommonConstants.PriceRoundDecimalPoints);
        }
    }

    private async Task EnhanceWithOpponents(IReadOnlyCollection<AppReservationVM> resultData, int currentUserId)
    {
        var hostedReservations = resultData.Where(x => x.HostId == currentUserId)
            .Select(x => new { OpponentId = x.AttenderId, Reservation = x });
        var guestedReservations = resultData.Where(x => x.AttenderId == currentUserId)
            .Select(x => new { OpponentId = x.HostId, Reservation = x });

        var allReservations = hostedReservations.Concat(guestedReservations).ToList();
        var userIds = allReservations.Select(x => x.OpponentId).ToHashSet();
        var userDictionary = await _appUserRepository.GetUserDictionary(userIds);
        foreach (var reservation in allReservations)
        {
            reservation.Reservation.Opponent = userDictionary[reservation.OpponentId];
        }
    }

    private async Task EnhanceWithRooms(IReadOnlyCollection<AppReservationVM> resultData)
    {
        var now = _timeUtility.GetUtcNow();
        var roomIds = resultData.FilterFutureReservations(now).Select(x => x.RoomId).ToHashSet();
        var roomDictionary = await _appRoomRepository.GetRoomDictionary(roomIds);
        foreach (var appReservationVm in resultData.Where(x => roomDictionary.ContainsKey(x.RoomId)))
        {
            appReservationVm.Room = roomDictionary[appReservationVm.RoomId];
        }
    }

    private async Task EnhanceWithReviews(IReadOnlyCollection<AppReservationVM> resultData)
    {
        var reviews =
            await _appUserReservationReviewRepository.GetReviewsByReservationId(
                resultData.Select(x => x.ReservationId));

        foreach (var appReservationVm in resultData)
        {
            appReservationVm.ReservationReview = reviews[appReservationVm.ReservationId].FirstOrDefault();
        }
    }

    private void EnhanceWithCanCancel(IEnumerable<AppReservationVM> entities)
    {
        foreach (var appReservationVm in entities)
        {
            appReservationVm.CanCancel = CanCancel(appReservationVm);
        }
    }

    private void EnhanceWithCanReschedule(IEnumerable<AppReservationVM> entities)
    {
        foreach (var appReservationVm in entities)
        {
            appReservationVm.CanReschedule = CanReschedule(appReservationVm);
        }
    }
}