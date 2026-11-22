using System.Security.Claims;
using Stripe.Checkout;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Utils;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppCoupon;
using ULearn.Be.Models.ViewModels.AppTimeslot;
using ULearn.Be.Models.ViewModels.Calendar;
using ULearn.Be.Models.ViewModels.Calendar.Base;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.Video.Interfaces;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppReservationService : IAppReservationService
{
    private readonly IAppConversationRepository _appConversationRepository;
    private readonly IAppCouponSharedService _appCouponSharedService;
    private readonly IAppRecurrentTimeslotBlacklistRepository _appRecurrentTimeslotBlacklistRepository;
    private readonly IAppReservationPackageRepository _appReservationPackageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppReservationSharedService _appReservationSharedService;
    private readonly IAppRoomRepository _appRoomRepository;
    private readonly IAppTimeslotRepository _appTimeslotRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly ICalendarSharedService _calendarSharedService;
    private readonly ICClassDurationRepository _cClassDurationRepository;
    private readonly ICCurrencyRepository _cCurrencyRepository;
    private readonly InstanceSettings _instanceSettings;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly StripeSettings _stripeSettings;
    private readonly ITimeUtility _timeUtility;
    private readonly ICTranslationRepository _translationRepository;
    private readonly IVideoCallProvider _videoCallProvider;

    public AppReservationService(
        IEnvConfigurationProvider envConfigurationProvider,
        IAppReservationRepository appReservationRepository,
        IAppUserRepository appUserRepository,
        IAppRoomRepository appRoomRepository,
        ICalendarSharedService calendarSharedService,
        IAppRecurrentTimeslotBlacklistRepository appRecurrentTimeslotBlacklistRepository,
        IAppTimeslotRepository appTimeslotRepository,
        IAppReservationSharedService appReservationSharedService,
        ITimeUtility timeUtility,
        IAppReservationPackageRepository appReservationPackageRepository,
        IAppUserWalletRepository appUserWalletRepository,
        ICTranslationRepository translationRepository,
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletSharedService appUserWalletSharedService,
        IAppConversationRepository appConversationRepository,
        IVideoCallProvider videoCallProvider,
        INotificationSharedService notificationSharedService,
        ICClassDurationRepository cClassDurationRepository,
        IAppCouponSharedService appCouponSharedService,
        ICCurrencyRepository cCurrencyRepository
    )
    {
        _appReservationRepository = appReservationRepository;
        _appUserRepository = appUserRepository;
        _appRoomRepository = appRoomRepository;
        _calendarSharedService = calendarSharedService;
        _appRecurrentTimeslotBlacklistRepository = appRecurrentTimeslotBlacklistRepository;
        _appTimeslotRepository = appTimeslotRepository;
        _appReservationPackageRepository = appReservationPackageRepository;
        _appUserWalletRepository = appUserWalletRepository;
        _translationRepository = translationRepository;
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletSharedService = appUserWalletSharedService;
        _appConversationRepository = appConversationRepository;
        _videoCallProvider = videoCallProvider;
        _notificationSharedService = notificationSharedService;
        _cClassDurationRepository = cClassDurationRepository;
        _appCouponSharedService = appCouponSharedService;
        _cCurrencyRepository = cCurrencyRepository;
        _appReservationSharedService = appReservationSharedService;
        _timeUtility = timeUtility;
        _instanceSettings = envConfigurationProvider.GetSettings<InstanceSettings>(EnvSettingsKeyEnum.InstanceSettings);
        _stripeSettings = envConfigurationProvider.GetSettings<StripeSettings>(EnvSettingsKeyEnum.StripeSettings);
    }

    public async Task<AppReservationCreateVM> Create(AppReservationInputVM inputVm, ClaimsPrincipal user)
    {
        var coupon = await GetCoupon(inputVm.CouponCode, user.GetUserId());
        var (
            totalPrice,
            firstItemPrice,
            otherItemPrice,
            itemPriceWithoutSales
            ) = await CalculatePrice(
            inputVm.TutorId,
            inputVm.FromDateTimes.Count,
            inputVm.DurationMinutes,
            coupon
        );
        var classDuration = await _cClassDurationRepository.Get((int)inputVm.DurationMinutes);

        var reservationPackageId = await _appReservationPackageRepository.Create(
            tutorId: inputVm.TutorId,
            studentId: user.GetUserId(),
            totalPrice: totalPrice,
            durationEnum: inputVm.DurationMinutes
        );
        await UpdateUserStateRoleSet(user.GetUserId());
        foreach (var (idx, reservationStartTime) in inputVm.FromDateTimes.Select((x, idx) => (idx, x)))
        {
            await Create(
                packageId: reservationPackageId,
                userId: user.GetUserId(),
                tutorId: inputVm.TutorId,
                price: idx == 0 ? firstItemPrice : otherItemPrice,
                priceWithoutSales: itemPriceWithoutSales,
                reservationStartTime: reservationStartTime,
                reservationEndTime: reservationStartTime.AddMinutes(classDuration.DurationWithMargin),
                reservationExpectedEndTime: reservationStartTime.AddMinutes(classDuration.ExpectedDuration),
                stateEnum: CReservationStateEnum.Created,
                durationEnum: inputVm.DurationMinutes,
                couponId: idx == 0 ? coupon?.CouponId : null,
                couponSaleAmount: idx == 0 && coupon != null ? otherItemPrice - firstItemPrice : null
            );
        }

        await _appConversationRepository.Upsert(user.GetUserId(), inputVm.TutorId);
        if (inputVm.UseWalletCredits is true && await HasEnoughCreditsToPay(reservationPackageId, user.GetUserId()))
        {
            await CompleteCreationFromCredits(reservationPackageId, user.GetUserId());
            return new AppReservationCreateVM
            {
                PayedFullyByCredits = true,
                RedirectUrl = null
            };
        }

        var session = await CreateReservationPaymentSession(reservationPackageId, inputVm, user.GetUserId());
        return new AppReservationCreateVM
        {
            PayedFullyByCredits = false,
            RedirectUrl = session.Url
        };
    }

    public async Task Edit(int reservationId, AppReservationEditVM inputVm, ClaimsPrincipal user)
    {
        var reservation = await _appReservationRepository.GetRawDbEntity(reservationId);
        var classDuration = await _cClassDurationRepository.Get(reservation.ClassDurationId);
        ThrowIfCannotEdit(reservation, user.GetUserId());
        var canEdit = _appReservationSharedService.CanReschedule(reservation);
        if (!canEdit)
        {
            throw new ForbiddenException();
        }

        await SoftDelete(reservationId, user);
        var newReservationId = await Create(
            packageId: reservation.ReservationPackageId,
            userId: reservation.AttenderId,
            tutorId: reservation.HostId,
            price: reservation.Price,
            priceWithoutSales: reservation.PriceWithoutSales,
            reservationStartTime: inputVm.NewFromDate,
            reservationEndTime: inputVm.NewFromDate + TimeSpan.FromMinutes(classDuration.DurationWithMargin),
            reservationExpectedEndTime: inputVm.NewFromDate + TimeSpan.FromMinutes(classDuration.ExpectedDuration),
            stateEnum: CReservationStateEnum.Confirmed,
            durationEnum: (CClassDurationEnum)reservation.ClassDurationId,
            sourceReservationId: reservationId,
            couponId: reservation.CouponId,
            couponSaleAmount: reservation.CouponSaleAmount
        );
        await _appTransactionRepository.UpdateStateByReservationId(
            reservationId,
            CTransactionStateEnum.Canceled,
            user.GetUserId()
        );
        await CreateRescheduleTransactions(reservationId, newReservationId, user);
        var opponentId = reservation.AttenderId == user.GetUserId() ? reservation.HostId : reservation.AttenderId;
        await _notificationSharedService.SendReservationRescheduled(newReservationId, user.GetUserId(), opponentId);
    }

    public Task<PagedVM<AppReservationVM>> Filter(FilterView<AppReservationFilter> filter, ClaimsPrincipal user)
    {
        return _appReservationSharedService.Filter(filter, user);
    }

    private async Task UpdateUserStateRoleSet(int userId)
    {
        var user = await _appUserRepository.GetRawDbEntity(userId);
        if (user.UserStateId == (int)CUserStateEnum.RoleSetup)
        {
            await _appUserRepository.UpdateState(userId, CUserStateEnum.Registration);
        }
    }

    private async Task CreateRescheduleTransactions(
        int oldReservationId,
        int newReservationId,
        ClaimsPrincipal principal
    )
    {
        var userId = principal.GetUserId();
        var now = _timeUtility.GetUtcNow();
        var oldTransactions = await _appTransactionRepository.GetRawDbEntitiesByReservationId(oldReservationId);
        var oldReservation = await _appReservationRepository.GetRawDbEntity(oldReservationId);
        var student = await _appUserRepository.GetRawDbEntity(oldReservation.AttenderId);
        var customerId = student.InviterId ?? student.UserId;
        var newReservation = await _appReservationRepository.GetRawDbEntity(newReservationId);
        var tutorWalletId = await _appUserWalletRepository.GetUserWalletId(oldReservation.HostId);
        var studentWalletId = await _appUserWalletRepository.GetUserWalletId(oldReservation.AttenderId);
        var technicalWalletId = await _appUserWalletRepository.GetUserWalletId(CommonConstants.TechnicalAdminUserId);
        var refundReasonEnum = principal.IsStudent()
            ? CTransactionRefundReasonEnum.StudentRescheduled
            : CTransactionRefundReasonEnum.TutorRescheduled;

        await _appTransactionRepository.AddRange(new List<AppTransactionGroupIM>
        {
            new(
                new AppTransactionIM(
                    ownerWalletId: studentWalletId,
                    vendorId: oldReservation.HostId,
                    customerId: customerId,
                    amount: oldReservation.Price,
                    reservationId: oldReservation.ReservationId,
                    reservationPackageId: oldReservation.ReservationPackageId,
                    reservationIssueId: null,
                    transactionTypeEnum: CTransactionTypeEnum.RefundCreditMemo,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    creatorId: userId,
                    transactionRefundReasonEnum: refundReasonEnum,
                    plannedCompletionTime: now,
                    sourceTransactionId: oldTransactions
                        .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonPayment)
                        .Where(x => x.UserWalletId == studentWalletId)
                        .Select(x => (int?)x.TransactionId)
                        .FirstOrDefault()
                ),
                new AppTransactionIM(
                    ownerWalletId: tutorWalletId,
                    vendorId: oldReservation.HostId,
                    customerId: customerId,
                    amount: -oldReservation.Price,
                    reservationId: oldReservation.ReservationId,
                    reservationPackageId: oldReservation.ReservationPackageId,
                    reservationIssueId: null,
                    transactionTypeEnum: CTransactionTypeEnum.RefundCreditMemo,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    creatorId: userId,
                    transactionRefundReasonEnum: refundReasonEnum,
                    plannedCompletionTime: now,
                    sourceTransactionId: oldTransactions
                        .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonPayment)
                        .Where(x => x.UserWalletId == tutorWalletId)
                        .Select(x => (int?)x.TransactionId)
                        .FirstOrDefault()
                )
            ),
            new(
                new AppTransactionIM
                (
                    ownerWalletId: tutorWalletId,
                    vendorId: CommonConstants.TechnicalAdminUserId,
                    customerId: oldReservation.HostId,
                    amount: PriceUtil.GetOurPriceCut(oldReservation.Price, oldReservation.CouponSaleAmount),
                    reservationId: oldReservation.ReservationId,
                    reservationPackageId: oldReservation.ReservationPackageId,
                    reservationIssueId: null,
                    transactionTypeEnum: CTransactionTypeEnum.RefundLessonCommissionCreditMemo,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    creatorId: userId,
                    transactionRefundReasonEnum: refundReasonEnum,
                    plannedCompletionTime: now,
                    sourceTransactionId: oldTransactions
                        .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonCommission)
                        .Where(x => x.UserWalletId == tutorWalletId)
                        .Select(x => (int?)x.TransactionId)
                        .FirstOrDefault()
                ),
                new AppTransactionIM
                (
                    ownerWalletId: technicalWalletId,
                    vendorId: CommonConstants.TechnicalAdminUserId,
                    customerId: oldReservation.HostId,
                    amount: -PriceUtil.GetOurPriceCut(oldReservation.Price, oldReservation.CouponSaleAmount),
                    reservationId: oldReservation.ReservationId,
                    reservationPackageId: oldReservation.ReservationPackageId,
                    reservationIssueId: null,
                    transactionTypeEnum: CTransactionTypeEnum.RefundLessonCommissionCreditMemo,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    creatorId: userId,
                    transactionRefundReasonEnum: refundReasonEnum,
                    plannedCompletionTime: now,
                    sourceTransactionId: oldTransactions
                        .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.LessonCommission)
                        .Where(x => x.UserWalletId == technicalWalletId)
                        .Select(x => (int?)x.TransactionId)
                        .FirstOrDefault()
                )
            ),
            new(
                new AppTransactionIM
                (
                    ownerWalletId: studentWalletId,
                    vendorId: newReservation.HostId,
                    customerId: customerId,
                    amount: -newReservation.Price,
                    reservationId: newReservation.ReservationId,
                    reservationPackageId: newReservation.ReservationPackageId,
                    transactionTypeEnum: CTransactionTypeEnum.LessonPayment,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    creatorId: userId,
                    plannedCompletionTime: newReservation.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                ),
                new AppTransactionIM
                (
                    ownerWalletId: tutorWalletId,
                    vendorId: newReservation.HostId,
                    customerId: customerId,
                    amount: newReservation.Price,
                    reservationId: newReservation.ReservationId,
                    reservationPackageId: newReservation.ReservationPackageId,
                    transactionTypeEnum: CTransactionTypeEnum.LessonPayment,
                    transactionStateEnum: CTransactionStateEnum.Created,
                    creatorId: userId,
                    plannedCompletionTime: newReservation.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                )
            ),
            new(
                new AppTransactionIM
                (
                    ownerWalletId: tutorWalletId,
                    vendorId: CommonConstants.TechnicalAdminUserId,
                    customerId: newReservation.HostId,
                    amount: -PriceUtil.GetOurPriceCut(newReservation.Price, newReservation.CouponSaleAmount),
                    reservationId: newReservation.ReservationId,
                    reservationPackageId: newReservation.ReservationPackageId,
                    transactionTypeEnum: CTransactionTypeEnum.LessonCommission,
                    transactionStateEnum: CTransactionStateEnum.Created,
                    creatorId: userId,
                    plannedCompletionTime: newReservation.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                ),
                new AppTransactionIM
                (
                    ownerWalletId: technicalWalletId,
                    vendorId: CommonConstants.TechnicalAdminUserId,
                    customerId: newReservation.HostId,
                    amount: PriceUtil.GetOurPriceCut(newReservation.Price, newReservation.CouponSaleAmount),
                    reservationId: newReservation.ReservationId,
                    reservationPackageId: newReservation.ReservationPackageId,
                    transactionTypeEnum: CTransactionTypeEnum.LessonCommission,
                    transactionStateEnum: CTransactionStateEnum.Created,
                    creatorId: userId,
                    plannedCompletionTime: newReservation.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                )
            )
        });
    }

    private async Task CompleteCreationFromCredits(int reservationPackageId, int userId)
    {
        var package = await _appReservationPackageRepository.GetRawDbEntity(reservationPackageId);
        var student = await _appUserRepository.GetRawDbEntity(package.AttenderId);
        var customerId = student.InviterId ?? student.UserId;
        var reservations = await _appReservationRepository.GetAllRawByPackageId(reservationPackageId);
        var studentWalletId = await _appUserWalletRepository.GetUserWalletId(userId);
        var tutorWalletId = await _appUserWalletRepository.GetUserWalletId(package.HostId);
        var technicalWalletId = await _appUserWalletRepository.GetUserWalletId(CommonConstants.TechnicalAdminUserId);

        await _appReservationRepository.UpdateStateByPackageId(
            reservationPackageId,
            CReservationStateEnum.Confirmed
        );

        await _appTransactionRepository.AddRange(
            reservations
                .OrderBy(x => x.StartTime)
                .SelectMany(x =>
                    new List<AppTransactionGroupIM>
                    {
                        new(
                            new AppTransactionIM
                            (
                                amount: -x.Price,
                                reservationId: x.ReservationId,
                                vendorId: x.HostId,
                                customerId: customerId,
                                reservationPackageId: reservationPackageId,
                                reservationIssueId: null,
                                transactionTypeEnum: CTransactionTypeEnum.LessonPayment,
                                transactionStateEnum: CTransactionStateEnum.Completed,
                                ownerWalletId: studentWalletId,
                                creatorId: userId,
                                plannedCompletionTime: x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                            ),
                            new AppTransactionIM
                            (
                                amount: x.Price,
                                vendorId: x.HostId,
                                customerId: customerId,
                                reservationId: x.ReservationId,
                                reservationPackageId: reservationPackageId,
                                transactionTypeEnum: CTransactionTypeEnum.LessonPayment,
                                transactionStateEnum: CTransactionStateEnum.Created,
                                ownerWalletId: tutorWalletId,
                                creatorId: userId,
                                plannedCompletionTime: x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                            )
                        ),
                        new(
                            new AppTransactionIM
                            (
                                amount: -PriceUtil.GetOurPriceCut(x.Price, x.CouponSaleAmount),
                                vendorId: CommonConstants.TechnicalAdminUserId,
                                customerId: x.HostId,
                                reservationId: x.ReservationId,
                                reservationPackageId: reservationPackageId,
                                transactionTypeEnum: CTransactionTypeEnum.LessonCommission,
                                transactionStateEnum: CTransactionStateEnum.Created,
                                ownerWalletId: tutorWalletId,
                                creatorId: CommonConstants.TechnicalAdminUserId,
                                plannedCompletionTime: x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                            ),
                            new AppTransactionIM
                            (
                                amount: PriceUtil.GetOurPriceCut(x.Price, x.CouponSaleAmount),
                                vendorId: CommonConstants.TechnicalAdminUserId,
                                customerId: x.HostId,
                                reservationId: x.ReservationId,
                                reservationPackageId: reservationPackageId,
                                transactionTypeEnum: CTransactionTypeEnum.LessonCommission,
                                transactionStateEnum: CTransactionStateEnum.Created,
                                ownerWalletId: technicalWalletId,
                                creatorId: CommonConstants.TechnicalAdminUserId,
                                plannedCompletionTime: x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays)
                            )
                        )
                    }
                )
        );

        await _appUserWalletSharedService.UpdateWalletsStatistics(package.AttenderId, package.HostId,
            CommonConstants.TechnicalAdminUserId);
        await _notificationSharedService.SendNewReservation(reservationPackageId);
    }

    private async Task<bool> HasEnoughCreditsToPay(int reservationPackageId, int userId)
    {
        var balanceInWallet = await _appTransactionRepository.GetCurrentBalance(userId);
        var price = await _appReservationPackageRepository.GetTotalPrice(reservationPackageId);
        return balanceInWallet >= price;
    }

    private async Task<Session> CreateReservationPaymentSession(
        int reservationPackageId,
        AppReservationInputVM inputVm,
        int currentUserId
    )
    {
        var tutor = await _appUserRepository.GetById(inputVm.TutorId);
        var currentUser = await _appUserRepository.GetById(currentUserId);
        var languageCode = TranslationUtil.GetLanguageCode(currentUser.TranslationLanguageId);
        var tutorPhotoUrl = tutor.Files.Where(x => x.FileTypeId == (int)CFileTypeEnum.ProfilePhoto)
            .Select(x => ImageUtil.GetFileUrl(_instanceSettings.BeApiUrl, x.FilePath))
            .FirstOrDefault();
        var numberOfLessons = inputVm.FromDateTimes.Count;
        var amountToPay = await GetAmountToPay(reservationPackageId, currentUserId, inputVm.UseWalletCredits ?? false);
        var productName = await GetPackageProductName(currentUser.TranslationLanguageId, tutor.GetUserName(),
            numberOfLessons,
            inputVm.DurationMinutes);
        var convertedAmountToPay = await _cCurrencyRepository.ConvertToUserCurrency(amountToPay, inputVm.CurrencyCode);
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = productName,
                            Images = new List<string>
                            {
                                tutorPhotoUrl
                            }.Where(x => x != null).ToList()
                        },
                        UnitAmountDecimal = Convert.ToDecimal(convertedAmountToPay * 100),
                        Currency = inputVm.CurrencyCode.ToUpper(),
                    }
                }
            },
            Mode = "payment",
            Locale = languageCode,
            SuccessUrl = _stripeSettings.ReservationSuccessUrl,
            CancelUrl = _stripeSettings.ReservationCancelUrl,
            ExpiresAt = _timeUtility.GetUtcNow().AddMinutes(30),
            AutomaticTax = new SessionAutomaticTaxOptions
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { StripeConstants.UserId, $"{currentUserId}" },
                { StripeConstants.TutorId, $"{tutor.UserId}" },
                { StripeConstants.ReservationPackageId, $"{reservationPackageId}" },
                { StripeConstants.SessionTypeId, $"{(int)StripeSessionTypeEnum.ReservationPayment}" },
            },
            CustomerEmail = currentUser.Login
        };
        var service = new SessionService();
        return await service.CreateAsync(options);
    }

    private async Task<double> GetAmountToPay(int reservationPackageId, int currentUserId, bool useWalletCredits)
    {
        var packagePrice = await _appReservationPackageRepository.GetTotalPrice(reservationPackageId);
        if (!useWalletCredits)
        {
            return packagePrice;
        }

        var balanceInWallet = await _appTransactionRepository.GetCurrentBalance(currentUserId);
        return packagePrice - balanceInWallet;
    }

    private async Task<string> GetPackageProductName(
        int? translationLanguageId,
        string tutorName,
        int numberOfLessons,
        CClassDurationEnum duration
    )
    {
        var languageId = translationLanguageId ?? (int)CTranslationLanguageEnum.English;
        const string translationKey = TranslationConstants.StripePackageProductNameKey;
        return TranslationUtil.Translate(
            await _translationRepository.GetTranslationByKeyPrefix(languageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { TranslationUtil.CountParameter, $"{numberOfLessons}" },
                { "tutorName", tutorName },
                { "duration", duration == CClassDurationEnum.Full ? "50" : "25" },
            });
    }

    private static void ThrowIfCannotEdit(AppReservation reservation, int userId)
    {
        if (reservation.HostId != userId && reservation.AttenderId != userId)
        {
            throw new ForbiddenException();
        }
    }

    private async Task SoftDelete(int reservationId, ClaimsPrincipal user)
    {
        await _appReservationSharedService.CreateTimeslot(reservationId, user.GetUserId());
        await _appReservationRepository.UpdateState(new List<int> { reservationId }, CReservationStateEnum.Rescheduled);
    }

    private async Task<(double totalPrice, double firstItemPrice, double otherItemPrice, double itemPriceWithoutSales)>
        CalculatePrice(
            int tutorId,
            int numberOfHours,
            CClassDurationEnum inputVmDurationMinutes,
            AppCouponVM coupon
        )
    {
        var hourRate = await _appUserRepository.GetHourRate(tutorId);
        var durationCoefficient = inputVmDurationMinutes == CClassDurationEnum.Full ? 1 : 0.5;
        var baseRate = hourRate * durationCoefficient;
        var priceWithPackageSale = numberOfHours switch
        {
            >= 10 => Math.Round(baseRate - (baseRate * CommonConstants.SaleForTenLessons),
                CommonConstants.PriceRoundDecimalPoints),
            >= 5 => Math.Round(baseRate - (baseRate * CommonConstants.SaleForFiveLessons),
                CommonConstants.PriceRoundDecimalPoints),
            _ => baseRate
        };
        var firstItemPrice =
            priceWithPackageSale - (priceWithPackageSale * coupon?.DiscountPercentage ?? 0) / 100;
        var priceWithoutSales = Math.Round(baseRate, CommonConstants.PriceRoundDecimalPoints);
        var totalPrice = firstItemPrice + priceWithPackageSale * (numberOfHours - 1);

        return (totalPrice, firstItemPrice, priceWithPackageSale, priceWithoutSales);
    }

    private async Task<int> Create(
        int packageId,
        int userId,
        int tutorId,
        double price,
        double priceWithoutSales,
        DateTime reservationStartTime,
        DateTime reservationEndTime,
        DateTime reservationExpectedEndTime,
        CReservationStateEnum stateEnum,
        CClassDurationEnum durationEnum,
        int? sourceReservationId = null,
        int? couponId = null,
        double? couponSaleAmount = null
    )
    {
        var durationInMinutes = (int)Math.Round((reservationEndTime - reservationStartTime).TotalMinutes);
        var allSlots = await _calendarSharedService.GetAll(
            tutorId,
            reservationStartTime.Date,
            reservationEndTime.Date.AddDays(1)
        );

        var freeSlots = GetFreeSlots(allSlots, reservationStartTime, reservationEndTime).ToList();

        if (!freeSlots.Any())
        {
            throw new ConflictException($"Could not find a free event for tutor {tutorId}");
        }

        await SplitTimeslots(freeSlots, tutorId, reservationStartTime, reservationEndTime);
        var remoteRoom = await _videoCallProvider.CreateRoom(durationInMinutes);
        var room = await _appRoomRepository.Add(tutorId, userId, reservationStartTime, reservationEndTime,
            remoteRoom.RoomKey);
        var tutorSubjectId = await _appUserRepository.GetSubjectId(tutorId);
        return await _appReservationRepository.Add(new AppReservationIM
        {
            ReservationPackageId = packageId,
            StudentId = userId,
            TutorId = tutorId,
            SubjectId = tutorSubjectId,
            Price = price,
            PriceWithoutSales = priceWithoutSales,
            RoomId = room.RoomId,
            StartTime = reservationStartTime,
            EndTime = reservationEndTime,
            ExpectedEndTime = reservationExpectedEndTime,
            HostEventIanaTimezone = freeSlots.First().HostIanaTimezone,
            ReservationStateEnum = stateEnum,
            SourceReservationId = sourceReservationId,
            ClassDurationEnum = durationEnum,
            CouponId = couponId,
            CouponSaleAmount = couponSaleAmount
        });
    }

    private static IEnumerable<ICalendarBaseTimeslotVM> GetFreeSlots(
        IReadOnlyCollection<CalendarBaseEventVM> allSlots,
        DateTime reservationStartTime,
        DateTime reservationEndTime
    )
    {
        var durationInMinutes = (int)(reservationEndTime - reservationStartTime).TotalMinutes;
        return durationInMinutes > 30
            ? new List<ICalendarBaseTimeslotVM>
                {
                    GetFreeSlot(allSlots, reservationStartTime),
                    GetFreeSlot(allSlots, reservationStartTime.AddMinutes(30))
                }
                .GroupBy(x => x.FromTime).Select(x => x.First())
            : new List<ICalendarBaseTimeslotVM> { GetFreeSlot(allSlots, reservationStartTime) };
    }

    private static ICalendarBaseTimeslotVM GetFreeSlot(
        IEnumerable<CalendarBaseEventVM> allSlots,
        DateTime reservationStartTime
    )
    {
        return allSlots
            .Where(x => x.FromTime <= reservationStartTime)
            .Where(x => x.ToTime >= reservationStartTime.AddMinutes(30))
            .OfType<ICalendarBaseTimeslotVM>()
            .FirstOrDefault();
    }

    private async Task SplitTimeslots(IEnumerable<ICalendarBaseTimeslotVM> timeslots, int tutorId, DateTime from,
        DateTime to)
    {
        foreach (var calendarBaseTimeslotVm in timeslots)
        {
            await SplitTimeslots(calendarBaseTimeslotVm, tutorId, from, to);
        }
    }

    private async Task SplitTimeslots(ICalendarBaseTimeslotVM currentEvent, int tutorId, DateTime from, DateTime to)
    {
        ArgumentNullException.ThrowIfNull(currentEvent);
        ArgumentNullException.ThrowIfNull(currentEvent.HostIanaTimezone);
        var convertedEventFromTime =
            _timeUtility.ConvertFromUtcToLocalTime(currentEvent.HostIanaTimezone, currentEvent.FromTime);
        var convertedFromTime = _timeUtility.ConvertFromUtcToLocalTime(currentEvent.HostIanaTimezone, from);
        var convertedToTime = _timeUtility.ConvertFromUtcToLocalTime(currentEvent.HostIanaTimezone, to);
        var type = currentEvent.GetType();

        if (type == typeof(CalendarRecurrentTimeslotVM))
        {
            var recurrentTimeslot = (CalendarRecurrentTimeslotVM)currentEvent;
            await _appRecurrentTimeslotBlacklistRepository.Add(
                recurrentTimeslot.RecurrentTimeslotId,
                convertedFromTime.Year,
                convertedFromTime.Month,
                convertedFromTime.Day
            );
        }
        else if (type == typeof(CalendarTimeslotVM))
        {
            var timeslot = (CalendarTimeslotVM)currentEvent;
            await _appTimeslotRepository.Delete(timeslot.TimeslotId, tutorId);
        }
        else
        {
            throw new ArgumentException("split timeslot cannot be called on a reservation timeslot");
        }

        if (currentEvent.FromTime < from)
        {
            await _appTimeslotRepository.Add(new AppTimeslotInputVM
            {
                IanaTimezone = currentEvent.HostIanaTimezone,
                FromLocalYear = convertedEventFromTime.Year,
                FromLocalMonth = convertedEventFromTime.Month,
                FromLocalDay = convertedEventFromTime.Day,
                FromLocalHour = convertedEventFromTime.Hour,
                FromLocalMinute = convertedEventFromTime.Minute,
                DurationInMinutes = (int)(from - currentEvent.FromTime).Duration().TotalMinutes
            }, tutorId);
        }

        if (currentEvent.ToTime > to)
        {
            await _appTimeslotRepository.Add(new AppTimeslotInputVM
            {
                IanaTimezone = currentEvent.HostIanaTimezone,
                FromLocalYear = convertedToTime.Year,
                FromLocalMonth = convertedToTime.Month,
                FromLocalDay = convertedToTime.Day,
                FromLocalHour = convertedToTime.Hour,
                FromLocalMinute = convertedFromTime.Minute,
                DurationInMinutes = (int)(currentEvent.ToTime - to).Duration().TotalMinutes
            }, tutorId);
        }
    }

    private async Task<AppCouponVM> GetCoupon(string couponCode, int userId)
    {
        if (string.IsNullOrEmpty(couponCode))
        {
            return null;
        }

        var coupon = await _appCouponSharedService.GetInfo(couponCode, userId);
        return coupon.ReadyToUse ? coupon : null;
    }
}