using System.Security.Claims;
using Stripe.Checkout;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Utils;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.Wallet;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppUserWalletService : IAppUserWalletService
{
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly IAppUserWalletSharedService _appUserWalletSharedService;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly StripeSettings _stripeSettings;
    private readonly ITimeUtility _timeUtility;
    private readonly ICTranslationRepository _translationRepository;

    public AppUserWalletService(
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletRepository appUserWalletRepository,
        IAppUserRepository appUserRepository,
        IEnvConfigurationProvider envConfigurationProvider,
        ICTranslationRepository translationRepository,
        IAppUserWalletSharedService appUserWalletSharedService,
        INotificationSharedService notificationSharedService,
        ITimeUtility timeUtility
    )
    {
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletRepository = appUserWalletRepository;
        _appUserRepository = appUserRepository;
        _translationRepository = translationRepository;
        _appUserWalletSharedService = appUserWalletSharedService;
        _notificationSharedService = notificationSharedService;
        _timeUtility = timeUtility;
        _stripeSettings = envConfigurationProvider.GetSettings<StripeSettings>(EnvSettingsKeyEnum.StripeSettings);
    }

    public async Task<UserWalletDepositVM> DepositMoney(WalletDepositIM input, ClaimsPrincipal user)
    {
        var session = await CreateBalanceRefillPaymentSession(user.GetUserId(), input);
        return new UserWalletDepositVM
        {
            RedirectUrl = session.Url
        };
    }

    public async Task WithdrawMoney(WalletWithdrawalIM input, ClaimsPrincipal user)
    {
        var walletId = await _appUserWalletRepository.GetUserWalletId(user.GetUserId());
        var currentBalance = await _appTransactionRepository.GetCurrentBalance(user.GetUserId());
        var now = _timeUtility.GetUtcNow();
        if (currentBalance < input.Amount)
        {
            throw new ConflictException("Not enough balance");
        }

        await _appUserWalletRepository.UpdateBankAccountInformation(walletId, input.BankAccountNumber, input.BankCode);
        var sourceTransactionId = await _appTransactionRepository.Add(
            new AppTransactionGroupIM(
                new AppTransactionIM
                (
                    ownerWalletId: walletId,
                    vendorId: CommonConstants.TechnicalAdminUserId,
                    customerId: user.GetUserId(),
                    amount: -input.Amount,
                    transactionStateEnum: CTransactionStateEnum.Pending,
                    transactionTypeEnum: CTransactionTypeEnum.Withdrawal,
                    bankAccountNumber: input.BankAccountNumber,
                    bankCode: input.BankCode,
                    creatorId: user.GetUserId(),
                    plannedCompletionTime: now
                ))
        );

        if (user.IsStudent())
        {
            await _appTransactionRepository.Add(
                new AppTransactionGroupIM(
                    new AppTransactionIM
                    (
                        ownerWalletId: await _appUserWalletRepository.GetUserWalletId(CommonConstants
                            .TechnicalAdminUserId),
                        vendorId: CommonConstants.TechnicalAdminUserId,
                        customerId: user.GetUserId(),
                        transactionTypeEnum: CTransactionTypeEnum.WithdrawCommission,
                        transactionStateEnum: CTransactionStateEnum.Pending,
                        amount: PriceUtil.GetOurWithdrawPriceFee(input.Amount),
                        sourceTransactionId: sourceTransactionId,
                        creatorId: CommonConstants.TechnicalAdminUserId,
                        plannedCompletionTime: now
                    )
                )
            );
        }

        await _appUserWalletSharedService.UpdateWalletsStatistics(user.GetUserId());
        await _notificationSharedService.SendNewWithdrawalRequest(user.GetUserId());
    }

    public async Task<UserWalletVM> Get(ClaimsPrincipal user)
    {
        var userId = user.GetUserId();
        var result = await _appUserWalletRepository.Get(userId);

        result.RevenueThisMonth = await _appTransactionRepository.GetRevenueThisMonth(userId);
        result.RevenuePreviousMonth = await _appTransactionRepository.GetRevenuePreviousMonth(userId);
        result.RevenueThisYear = await _appTransactionRepository.GetRevenueThisYear(userId);
        result.RevenuePreviousYear = await _appTransactionRepository.GetRevenuePreviousYear(userId);
        result.WithdrawThisMonth = await _appTransactionRepository.GetWithdrawThisMonth(userId);
        result.WithdrawThisYear = await _appTransactionRepository.GetWithdrawThisYear(userId);

        return result;
    }

    public Task TopUpPayment(TopUpPaymentIM input, ClaimsPrincipal user)
    {
        return TopUp(user.GetUserId(), input.Amount);
    }

    public Task TopUpUserPayment(TopUpUserPaymentIM input, ClaimsPrincipal user)
    {
        return TopUp(user.GetUserId(), input.Amount, input.UserId);
    }

    public async Task ExtractMoney(ExtractPaymentIM input, ClaimsPrincipal user)
    {
        await ThrowIfInvalidEmployee(user.GetUserId(), input.UserId);
        await ThrowIfInvalidAmount(input.UserId, input.Amount);
        var adminId = user.GetUserId();
        var employeeId = input.UserId;
        var adminWalletId = await _appUserWalletRepository.GetUserWalletId(adminId);
        var employeeWalletId = await _appUserWalletRepository.GetUserWalletId(employeeId);
        var now = _timeUtility.GetUtcNow();

        await _appTransactionRepository.Add(
            new AppTransactionGroupIM(
                new AppTransactionIM(
                    creatorId: adminId,
                    vendorId: employeeId,
                    customerId: adminId,
                    ownerWalletId: adminWalletId,
                    transactionTypeEnum: CTransactionTypeEnum.MoneyTransfer,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    amount: input.Amount,
                    plannedCompletionTime: now
                ),
                new AppTransactionIM(
                    creatorId: adminId,
                    vendorId: employeeId,
                    customerId: adminId,
                    ownerWalletId: employeeWalletId,
                    transactionTypeEnum: CTransactionTypeEnum.MoneyTransfer,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    amount: -input.Amount,
                    plannedCompletionTime: now
                )
            ));
        await _appUserWalletSharedService.UpdateWalletsStatistics(adminId, input.UserId);
    }

    private async Task ThrowIfInvalidEmployee(int adminId, int colleagueId)
    {
        var exists = await _appUserRepository.ExistsEmployee(adminId, colleagueId);
        if (!exists)
        {
            throw new ConflictException(ErrorCodesEnum.InvalidEmployeeId);
        }
    }

    private async Task ThrowIfInvalidAmount(int userId, double amount)
    {
        var currentBalance = await _appTransactionRepository.GetCurrentBalance(userId);
        if (amount > currentBalance)
        {
            throw new ConflictException(ErrorCodesEnum.NotEnoughMoney);
        }
    }

    private async Task TopUp(int adminId, double amount, int? userId = null)
    {
        var employeeIds = (await _appUserRepository.GetEmployeeIds(adminId))
            .Where(x => !userId.HasValue || x == userId)
            .ToList();
        var totalAmount = amount * employeeIds.Count;
        await ThrowIfHasNotEnoughBalance(totalAmount, adminId);
        var adminWalletId = await _appUserWalletRepository.GetUserWalletId(adminId);
        var colleagueWalletIds = await _appUserWalletRepository.GetUserWalletByUserId(employeeIds);
        var now = _timeUtility.GetUtcNow();

        await _appTransactionRepository.AddRange(
            colleagueWalletIds.Select(x => new AppTransactionGroupIM(
                new AppTransactionIM(
                    creatorId: adminId,
                    vendorId: adminId,
                    customerId: x.Key,
                    ownerWalletId: adminWalletId,
                    transactionTypeEnum: CTransactionTypeEnum.MoneyTransfer,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    amount: -amount,
                    plannedCompletionTime: now
                ),
                new AppTransactionIM(
                    creatorId: adminId,
                    vendorId: adminId,
                    customerId: x.Key,
                    ownerWalletId: x.Value,
                    transactionTypeEnum: CTransactionTypeEnum.MoneyTransfer,
                    transactionStateEnum: CTransactionStateEnum.Completed,
                    amount: amount,
                    plannedCompletionTime: now
                )
            ))
        );

        await _appUserWalletSharedService.UpdateWalletsStatistics(employeeIds.Append(adminId));
        await _notificationSharedService.SendCompanySendMoney(adminId, amount, employeeIds.Count);
    }

    private async Task ThrowIfHasNotEnoughBalance(double totalAmount, int userId)
    {
        var currentBalance = await _appTransactionRepository.GetCurrentBalance(userId);
        if (totalAmount > currentBalance)
        {
            throw new ConflictException(ErrorCodesEnum.NotEnoughMoney);
        }
    }

    private async Task<Session> CreateBalanceRefillPaymentSession(int userId, WalletDepositIM input)
    {
        var currentUser = await _appUserRepository.GetById(userId);
        var languageCode = TranslationUtil.GetLanguageCode(currentUser.TranslationLanguageId);
        var languageId = currentUser.TranslationLanguageId ?? (int)CTranslationLanguageEnum.English;
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
                            Name = await GetBalanceRefillProductName(languageId),
                        },
                        UnitAmountDecimal =
                            Convert.ToDecimal(Math.Round(input.Amount, CommonConstants.PriceRoundDecimalPoints) * 100),
                        Currency = input.CurrencyCode.ToUpper(),
                    }
                }
            },
            Mode = "payment",
            Locale = languageCode,
            SuccessUrl = _stripeSettings.MoneyRefillSuccessUrl,
            CancelUrl = _stripeSettings.MoneyRefillCancelUrl,
            AutomaticTax = new SessionAutomaticTaxOptions
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { StripeConstants.UserId, $"{userId}" },
                { StripeConstants.SessionTypeId, $"{(int)StripeSessionTypeEnum.BalanceRefill}" }
            },
            CustomerEmail = currentUser.Login
        };
        var service = new SessionService();
        return await service.CreateAsync(options);
    }

    private async Task<string> GetBalanceRefillProductName(int languageId)
    {
        const string translationKey = TranslationConstants.BalanceRefillProductNameKey;
        var translations = await _translationRepository.GetTranslationByKeyPrefix(languageId, translationKey);
        return TranslationUtil.Translate(translations, translationKey);
    }
}