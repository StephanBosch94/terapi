using Microsoft.Extensions.Primitives;
using Stripe;
using Stripe.Checkout;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class CheckoutService : ICheckoutService
{
    private readonly IAppReservationPackageRepository _appReservationPackageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppReservationSharedService _appReservationSharedService;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly ICCurrencyRepository _cCurrencyRepository;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly IRUserActivatedCouponRepository _rUserActivatedCouponRepository;
    private readonly StripeSettings _stripeSettings;
    private readonly ITimeUtility _timeUtility;

    public CheckoutService(
        IEnvConfigurationProvider envConfigurationProvider,
        IAppReservationPackageRepository appReservationPackageRepository,
        IAppReservationRepository appReservationRepository,
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletRepository appUserWalletRepository,
        IAppReservationSharedService appReservationSharedService,
        IAppUserWalletSharedService appUserWalletSharedService,
        INotificationSharedService notificationSharedService,
        ITimeUtility timeUtility,
        IRUserActivatedCouponRepository rUserActivatedCouponRepository,
        ICCurrencyRepository cCurrencyRepository
    )
    {
        _appReservationPackageRepository = appReservationPackageRepository;
        _appReservationRepository = appReservationRepository;
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletRepository = appUserWalletRepository;
        _appReservationSharedService = appReservationSharedService;
        _appUserWalletSharedService = appUserWalletSharedService;
        _notificationSharedService = notificationSharedService;
        _timeUtility = timeUtility;
        _rUserActivatedCouponRepository = rUserActivatedCouponRepository;
        _cCurrencyRepository = cCurrencyRepository;
        _stripeSettings = envConfigurationProvider.GetSettings<StripeSettings>(EnvSettingsKeyEnum.StripeSettings);
    }

    public async Task WebhookHandle(string json, StringValues signature)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signature,
                _stripeSettings.WebhookKey
            );

            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted:
                    await HandleCheckoutCompleted(stripeEvent);
                    break;
                case Events.CheckoutSessionExpired:
                    await HandleCheckoutExpired(stripeEvent);
                    break;
            }
        }
        catch (StripeException e)
        {
            throw new ConflictException(e.Message);
        }
    }

    private async Task HandleCheckoutCompleted(Event stripeEvent)
    {
        var parameters = await GetSuccessParameters(stripeEvent);
        switch (parameters?.SessionTypeEnum)
        {
            case StripeSessionTypeEnum.BalanceRefill:
                await HandleBalanceRefillCompleted(parameters);
                break;
            case StripeSessionTypeEnum.ReservationPayment:
                await HandleReservationPaymentCompleted(parameters);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task HandleCheckoutExpired(Event stripeEvent)
    {
        var parameters = GetExpiredParameters(stripeEvent);

        switch (parameters?.SessionTypeEnum)
        {
            case StripeSessionTypeEnum.BalanceRefill:
                await HandleBalanceRefillExpired(parameters);
                break;
            case StripeSessionTypeEnum.ReservationPayment:
                await HandleReservationPaymentExpired(parameters);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static StripeExpiredResponseData GetExpiredParameters(Event stripeEvent)
    {
        if (stripeEvent?.Data?.Object is not Session session)
        {
            return null;
        }

        var metadata = session.Metadata;
        return new StripeExpiredResponseData
        {
            SessionId = session.Id,
            SessionTypeEnum = (StripeSessionTypeEnum)GetIntParameter(metadata, StripeConstants.SessionTypeId),
            Metadata = session.Metadata
        };
    }

    private static async Task<StripeSuccessResponseData> GetSuccessParameters(Event stripeEvent)
    {
        if (stripeEvent?.Data?.Object is not Session session)
        {
            return null;
        }

        var sessionWithLineItems = await GetItems(session);
        if (sessionWithLineItems == null)
        {
            return null;
        }

        var intent = await GetIntent(session);
        if (intent == null)
        {
            return null;
        }

        var metadata = sessionWithLineItems.Metadata;

        return new StripeSuccessResponseData
        {
            SessionId = session.Id,
            SessionTypeEnum = (StripeSessionTypeEnum)GetIntParameter(metadata, StripeConstants.SessionTypeId),
            TotalAmount =
                (sessionWithLineItems.LineItems?.Data?.Sum(x => decimal.ToDouble(x?.Price?.UnitAmountDecimal ?? 0)) ??
                 0) / 100,
            Metadata = metadata,
            Fees = (double)(intent.LatestCharge?.BalanceTransaction?.Fee ?? 0) / 100,
            ReceiptUrl = intent.LatestCharge?.ReceiptUrl,
            CurrencyCode = session.Currency
        };
    }

    private static Task<PaymentIntent> GetIntent(Session session)
    {
        if (string.IsNullOrEmpty(session?.PaymentIntentId))
        {
            return null;
        }

        var service = new PaymentIntentService();
        return service.GetAsync(
            session.PaymentIntentId,
            new PaymentIntentGetOptions
            {
                Expand = new List<string>
                {
                    "latest_charge.balance_transaction"
                }
            });
    }

    private static Task<Session> GetItems(Session session)
    {
        if (string.IsNullOrEmpty(session?.PaymentIntentId))
        {
            return null;
        }

        var service = new SessionService();
        return service.GetAsync(
            session.Id,
            new SessionGetOptions { Expand = new List<string> { "line_items" } }
        );
    }

    private async Task HandleBalanceRefillCompleted(StripeSuccessResponseData data)
    {
        var userId = GetIntParameter(data.Metadata, StripeConstants.UserId);
        var now = _timeUtility.GetUtcNow();
        var transactionId = await _appTransactionRepository.Add(
            new AppTransactionGroupIM(
                new AppTransactionIM
                (
                    amount: await _cCurrencyRepository.ConvertToInternalCurrency(data.TotalAmount, data.CurrencyCode),
                    vendorId: CommonConstants.TechnicalAdminUserId,
                    customerId: userId,
                    sessionId: data.SessionId,
                    transactionTypeEnum: CTransactionTypeEnum.Deposit,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    ownerWalletId: await _appUserWalletRepository.GetUserWalletId(userId),
                    creatorId: userId,
                    receiptUrl: data.ReceiptUrl,
                    plannedCompletionTime: now
                )
            ));
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);

        await _appTransactionRepository.Add(
            new AppTransactionIM
            (
                amount: -data.Fees,
                vendorId: CommonConstants.TechnicalAdminUserId, // stripe
                customerId: CommonConstants.TechnicalAdminUserId,
                sessionId: data.SessionId,
                transactionTypeEnum: CTransactionTypeEnum.StripeCommission,
                transactionStateEnum: CTransactionStateEnum.Completed,
                ownerWalletId: await _appUserWalletRepository.GetUserWalletId(CommonConstants.TechnicalAdminUserId),
                creatorId: CommonConstants.TechnicalAdminUserId,
                receiptUrl: data.ReceiptUrl,
                sourceTransactionId: transactionId,
                plannedCompletionTime: now
            ), transaction.TransactionGroupId);

        await _appUserWalletSharedService.UpdateWalletsStatistics(userId, CommonConstants.TechnicalAdminUserId);
        await _notificationSharedService.SendStudentDepositedMoney(transactionId);
    }

    private static Task HandleBalanceRefillExpired(StripeExpiredResponseData _)
    {
        // ignored
        return Task.CompletedTask;
    }

    private async Task HandleReservationPaymentCompleted(StripeSuccessResponseData data)
    {
        var reservationPackageId = GetIntParameter(data.Metadata, StripeConstants.ReservationPackageId);
        var package = await _appReservationPackageRepository.GetRawDbEntity(reservationPackageId);
        var reservations = await _appReservationRepository.GetAllRawByPackageId(reservationPackageId);
        var userId = GetIntParameter(data.Metadata, StripeConstants.UserId);
        var studentWalletId = await _appUserWalletRepository.GetUserWalletId(userId);
        var tutorWalletId = await _appUserWalletRepository.GetUserWalletId(package.HostId);
        var technicalWalletId = await _appUserWalletRepository.GetUserWalletId(CommonConstants.TechnicalAdminUserId);
        var now = _timeUtility.GetUtcNow();
        await _appReservationRepository.UpdateStateByPackageId(
            reservationPackageId,
            CReservationStateEnum.Confirmed
        );
        await MarkCouponsUsed(
            userId,
            reservations
                .Where(x => x.CouponId.HasValue)
                .Select(x => x.CouponId!.Value)
        );
        await _appTransactionRepository.AddRange(
            new List<AppTransactionGroupIM>
            {
                new(
                    new AppTransactionIM
                    (
                        amount: package.TotalPrice,
                        vendorId: CommonConstants.TechnicalAdminUserId,
                        customerId: userId,
                        sessionId: data.SessionId,
                        transactionTypeEnum: CTransactionTypeEnum.Deposit,
                        transactionStateEnum: CTransactionStateEnum.Completed,
                        ownerWalletId: studentWalletId,
                        creatorId: userId,
                        receiptUrl: data.ReceiptUrl,
                        plannedCompletionTime: now
                    )
                ),
                new(
                    new AppTransactionIM
                    (
                        amount: -data.Fees,
                        vendorId: CommonConstants.TechnicalAdminUserId,
                        customerId: userId,
                        sessionId: data.SessionId,
                        transactionTypeEnum: CTransactionTypeEnum.StripeCommission,
                        transactionStateEnum: CTransactionStateEnum.Completed,
                        ownerWalletId: technicalWalletId,
                        creatorId: userId,
                        receiptUrl: data.ReceiptUrl,
                        plannedCompletionTime: now
                    )
                )
            }.Concat(
                reservations
                    .OrderBy(x => x.StartTime)
                    .SelectMany(x => new List<AppTransactionGroupIM>
                    {
                        new(
                            new AppTransactionIM
                            (
                                amount: -x.Price,
                                vendorId: x.HostId,
                                customerId: x.AttenderId,
                                reservationId: x.ReservationId,
                                reservationPackageId: reservationPackageId,
                                transactionTypeEnum: CTransactionTypeEnum.LessonPayment,
                                transactionStateEnum: CTransactionStateEnum.Completed,
                                ownerWalletId: studentWalletId,
                                creatorId: userId,
                                plannedCompletionTime: x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays),
                                couponId: x.CouponId,
                                couponSaleAmount: x.CouponSaleAmount
                            ),
                            new AppTransactionIM
                            (
                                amount: x.Price,
                                vendorId: x.HostId,
                                customerId: x.AttenderId,
                                reservationId: x.ReservationId,
                                reservationPackageId: reservationPackageId,
                                transactionTypeEnum: CTransactionTypeEnum.LessonPayment,
                                transactionStateEnum: CTransactionStateEnum.Created,
                                ownerWalletId: tutorWalletId,
                                creatorId: userId,
                                plannedCompletionTime: x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays),
                                couponId: x.CouponId,
                                couponSaleAmount: x.CouponSaleAmount
                            )
                        ),
                        !x.CouponId.HasValue || PriceUtil.GetOurPriceCut(x.Price, x.CouponSaleAmount) > 0
                            ? new AppTransactionGroupIM(
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
                                    plannedCompletionTime: x.EndTime.AddDays(
                                        CommonConstants.MinimumTimeForPaymentInDays)
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
                                    plannedCompletionTime: x.EndTime.AddDays(
                                        CommonConstants.MinimumTimeForPaymentInDays)
                                )
                            )
                            : null
                    }.Where(y => y != null)))
        );

        await _appUserWalletSharedService.UpdateWalletsStatistics(
            new List<int>
            {
                CommonConstants.TechnicalAdminUserId,
                package.HostId,
                package.AttenderId
            });
        await _notificationSharedService.SendNewReservation(reservationPackageId);
    }

    private Task MarkCouponsUsed(int userId, IEnumerable<int> couponIds)
    {
        return _rUserActivatedCouponRepository.Add(userId, couponIds);
    }

    private async Task HandleReservationPaymentExpired(StripeCommonResponseData data)
    {
        var reservationPackageId = GetIntParameter(data.Metadata, StripeConstants.ReservationPackageId);
        var reservationIds = await _appReservationRepository.GetAllReservationIdsByPackageId(reservationPackageId);
        var tutorId = GetIntParameter(data.Metadata, StripeConstants.TutorId);

        foreach (var remainingReservationId in reservationIds)
        {
            await _appReservationSharedService.CreateTimeslot(remainingReservationId, tutorId);
        }

        await _appReservationRepository.UpdateStateByPackageId(
            reservationPackageId,
            CReservationStateEnum.FailedPayment
        );
        await _appReservationPackageRepository.Cancel(reservationPackageId, "Payment timeout");
    }


    private static int GetIntParameter(IReadOnlyDictionary<string, string> metadata, string parameterName)
    {
        if (metadata == null || !metadata.ContainsKey(parameterName))
        {
            throw new ConflictException($"Cannot find [{parameterName}] in session metadata");
        }

        if (!int.TryParse(metadata[parameterName], out var intValue))
        {
            throw new ConflictException($"Cannot parse [{parameterName}] in metadata");
        }

        return intValue;
    }

    private record StripeCommonResponseData
    {
        public StripeSessionTypeEnum SessionTypeEnum { get; set; }
        public string SessionId { get; set; }
        public IReadOnlyDictionary<string, string> Metadata { get; set; }
    }

    private record StripeExpiredResponseData : StripeCommonResponseData;

    private record StripeSuccessResponseData : StripeCommonResponseData
    {
        public double TotalAmount { get; set; }
        public double Fees { get; set; }
        public string ReceiptUrl { get; set; }
        public string CurrencyCode { get; set; }
    }
}