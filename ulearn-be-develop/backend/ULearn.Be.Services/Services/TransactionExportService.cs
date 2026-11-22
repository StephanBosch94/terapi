using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using ChromiumHtmlToPdfLib;
using ChromiumHtmlToPdfLib.Settings;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Mustachio;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Utils;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Repositories;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class TransactionExportService : ITransactionExportService
{
    private const int LanguageId = (int)CTranslationLanguageEnum.English;
    private readonly IAppCouponRepository _appCouponRepository;
    private readonly IAppReservationPackageRepository _appReservationPackageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;
    private readonly ICClassDurationRepository _cClassDurationRepository;
    private readonly ICTransactionRefundReasonRepository _cTransactionRefundReasonRepository;
    private readonly ICTransactionStateRepository _cTransactionStateRepository;
    private readonly ICTransactionTemplateRepository _cTransactionTemplateRepository;
    private readonly ICTranslationRepository _cTranslationRepository;
    private readonly IReadOnlyDictionary<CTransactionTypeEnum, Func<int, int, Task<ExportDataVM>>> _dataFunctions;
    private readonly ILogger<TransactionExportService> _logger;
    private readonly ITimeUtility _timeUtility;

    public TransactionExportService(
        IAppTransactionRepository appTransactionRepository,
        IAppUserWalletRepository appUserWalletRepository,
        ICTransactionTemplateRepository cTransactionTemplateRepository,
        ILogger<TransactionExportService> logger,
        IAppUserRepository appUserRepository,
        IAppReservationRepository appReservationRepository,
        ICTranslationRepository cTranslationRepository,
        IAppReservationPackageRepository appReservationPackageRepository,
        ICClassDurationRepository cClassDurationRepository,
        ICTransactionStateRepository cTransactionStateRepository,
        ITimeUtility timeUtility,
        ICTransactionRefundReasonRepository cTransactionRefundReasonRepository,
        IAppCouponRepository appCouponRepository
    )
    {
        _appUserWalletRepository = appUserWalletRepository;
        _cTransactionTemplateRepository = cTransactionTemplateRepository;
        _logger = logger;
        _appTransactionRepository = appTransactionRepository;
        _appUserRepository = appUserRepository;
        _appReservationRepository = appReservationRepository;
        _cTranslationRepository = cTranslationRepository;
        _appReservationPackageRepository = appReservationPackageRepository;
        _cClassDurationRepository = cClassDurationRepository;
        _cTransactionStateRepository = cTransactionStateRepository;
        _timeUtility = timeUtility;
        _cTransactionRefundReasonRepository = cTransactionRefundReasonRepository;
        _appCouponRepository = appCouponRepository;


        _dataFunctions = new Dictionary<CTransactionTypeEnum, Func<int, int, Task<ExportDataVM>>>
        {
            { CTransactionTypeEnum.PackageCancellationCreditMemo, ExportPackageCancellation },
            { CTransactionTypeEnum.Withdrawal, ExportWithdrawal },
            { CTransactionTypeEnum.RefundCreditMemo, ExportRefund },
            { CTransactionTypeEnum.LessonPayment, ExportLessonPayment },
            { CTransactionTypeEnum.LessonCommission, ExportLessonCommission },
            { CTransactionTypeEnum.RefundLessonCommissionCreditMemo, ExportRefundLessonCommissionCreditMemo },
            { CTransactionTypeEnum.WithdrawCommission, ExportWithdrawFee },
            { CTransactionTypeEnum.PackageSaleCancellationCommission, ExportPackageSaleCancellationCommission },
            { CTransactionTypeEnum.MoneyTransfer, ExportMoneyTransfer }
        };
    }


    public async Task<AppTransactionPdfFileVM> ExportToPdf(int transactionId, ClaimsPrincipal user)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        await ThrowIfNotBelongToUser(transaction, user.GetUserId());
        var function = TryGetFunction((CTransactionTypeEnum)transaction.TransactionTypeId);
        return await ExportData(transactionId, await function(transactionId, user.GetUserId()));
    }

    public async Task<AppTransactionCsvFileVM> ExportAllToCsv(AppTransactionExportIM options, ClaimsPrincipal user)
    {
        var translations = await GetTranslations(user);
        var exportData = await GetExportData(options, user, translations);
        return new AppTransactionCsvFileVM
        {
            Name = GetCsvFileName(),
            Data = ExportToCsv(exportData, GetLocalizedCsvHeaders(translations))
        };
    }

    private async Task<IReadOnlyDictionary<string, string>> GetTranslations(ClaimsPrincipal user)
    {
        var userInfo = await _appUserRepository.GetById(user.GetUserId());
        return await _cTranslationRepository.GetTranslationDictionary(userInfo.TranslationLanguageId);
    }

    private static byte[] ExportToCsv(IEnumerable<AppTransactionExportVM> dataList,
        IReadOnlyDictionary<string, string> localizedHeaders)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        csv.Context.RegisterClassMap(new AppTransactionExportClassMap(localizedHeaders));
        csv.WriteRecords(dataList);

        writer.Flush(); // Perhaps not necessary, but CsvWriter's documentation does not mention whether the underlying stream gets flushed or not

        stream.Position = 0;
        return stream.ToArray();
    }

    private async Task<IReadOnlyCollection<AppTransactionExportVM>> GetExportData(
        AppTransactionExportIM options,
        ClaimsPrincipal user,
        IReadOnlyDictionary<string, string> translations
    )
    {
        var data = await _appTransactionRepository.GetExportData(options, user.GetUserId());

        var results = data.Select(x =>
            new AppTransactionExportVM
            {
                InvoiceNumber = GetInvoiceNumber(x.TransactionGroupId),
                ReservationNumber = x.Reservation?.ReservationId,
                Transaction = TranslationUtil.Translate(translations, x.Type.NameKey),
                Status = TranslationUtil.Translate(translations, x.State.NameKey),
                Amount = PriceUtil.GetRoundedPrice(x.Amount).ToString(CultureInfo.InvariantCulture),
                AmountCurrency = "Kč",
                CustomerName = x.Customer.GetUserName(),
                CustomerIco = x.Customer.Ico,
                CustomerDic = x.Customer.Dic,
                VendorName = x.Vendor.GetUserName(),
                VendorIco = x.Vendor.Ico,
                VendorDic = x.Vendor.Dic,
                DueDate = x.Created.ToString("yyyy-MM-dd"),
                TaxDate = (x.Reservation?.EndTime ?? x.Created).ToString("yyyy-MM-dd")
            }).ToList();

        return results;
    }

    private Func<int, int, Task<ExportDataVM>> TryGetFunction(CTransactionTypeEnum transactionTypeEnum)
    {
        _dataFunctions.TryGetValue(transactionTypeEnum, out var function);
        if (function == null)
        {
            throw new ConflictException(ErrorCodesEnum.NotValidTransactionType);
        }

        return function;
    }

    private async Task<ExportDataVM> ExportPackageCancellation(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var refundReason =
            await _cTransactionRefundReasonRepository.GetRawDbEntity(transaction.TransactionRefundReasonId!.Value);
        var package = await _appReservationPackageRepository.GetRawDbEntity(transaction.ReservationPackageId!.Value);
        var classDuration = await _cClassDurationRepository.Get(package.ClassDurationId);
        var allReservations = await _appReservationRepository.GetAllRawByPackageId(
            package.ReservationPackageId
        );
        var cancelledReservations = allReservations
            .Where(x => x.ReservationStateId == (int)CReservationStateEnum.CanceledByUser)
            .ToList();
        var cancelledTransactions = await _appTransactionRepository.GetRawDbEntitiesByReservationIdsAndOwnerId(
            cancelledReservations.Select(x => x.ReservationId),
            package.AttenderId,
            CTransactionTypeEnum.LessonPayment
        );
        var completedReservations = allReservations
            .Where(x => AppReservationRepository.ConfirmedStates.Contains(x.ReservationStateId))
            .ToList();
        var vendor = await _appUserRepository.GetById(package.HostId);
        var customer = await GetCustomer(package.AttenderId);
        var description = await GetLessonDescription(
            vendor.GetUserName(),
            classDuration.ExpectedDuration
        );
        var feeDescription = await GetPackageCancellationFeeDescription(completedReservations.Count);
        var salesDiffAmount = completedReservations.Sum(x => x.PriceWithoutSales - x.Price);
        var additionalData = salesDiffAmount > 0
            ? new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.FeeCount, 1 },
                { CTransactionTemplateDataEnum.FeeDescription, feeDescription },
                { CTransactionTemplateDataEnum.FeeAmount, PriceUtil.GetPriceString(salesDiffAmount) }
            }
            : new Dictionary<CTransactionTemplateDataEnum, object>();

        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.TutorsCreditMemo,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                {
                    CTransactionTemplateDataEnum.Id,
                    GetRelatedIdsList(cancelledTransactions.Select(x => x.TransactionGroupId))
                },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.UserName, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.PlaceOfBusiness, vendor.PlaceOfBusiness },
                { CTransactionTemplateDataEnum.Ico, vendor.Ico },
                { CTransactionTemplateDataEnum.Count, cancelledReservations.Count },
                { CTransactionTemplateDataEnum.ProductDescription, description },
                { CTransactionTemplateDataEnum.CustomerUserName, customer.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, customer.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, customer.Dic },
                { CTransactionTemplateDataEnum.Reason, await GetTranslation(refundReason.NameKey) },
                {
                    CTransactionTemplateDataEnum.Price,
                    PriceUtil.GetPriceString(-Math.Abs(cancelledTransactions.Sum(x => x.Amount) /
                                                       cancelledReservations.Count))
                },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(-Math.Abs(transaction.Amount)) },
            }.Concat(additionalData).ToDictionary(x => x.Key, x => x.Value)
        };
    }

    private async Task<ExportDataVM> ExportWithdrawal(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        return await ExportWithdrawTransaction(transaction.TransactionId, transaction.Created, transaction.Amount);
    }

    private async Task<ExportDataVM> ExportLessonPayment(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var reservation = await _appReservationRepository.GetRawDbEntity(transaction.ReservationId!.Value);
        var classDuration = await _cClassDurationRepository.Get(reservation.ClassDurationId);
        var vendor = await _appUserRepository.GetById(reservation.HostId);
        var customer = await GetCustomer(reservation.AttenderId);

        var description = await GetLessonDescription(
            vendor.GetUserName(),
            classDuration.ExpectedDuration
        );
        var (priceBase, tax) = PriceUtil.GetPrice(
            Math.Abs(transaction.Amount) + (transaction.CouponSaleAmount ?? 0), // full price without a sale
            vendor.IsCountryTaxPayer
        );
        var coupon = transaction.CouponId.HasValue
            ? await _appCouponRepository.GetRawDbEntity(transaction.CouponId.Value)
            : null;
        var couponData = coupon != null
            ? new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.CouponCount, 1 },
                {
                    CTransactionTemplateDataEnum.CouponAmount,
                    PriceUtil.GetPriceString(-Math.Abs(transaction.CouponSaleAmount ?? 0))
                },
                {
                    CTransactionTemplateDataEnum.CouponDescription,
                    await GetCouponShortDescription(coupon.DiscountPercentage)
                },
            }
            : new Dictionary<CTransactionTemplateDataEnum, object>();
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = GetTutorsTemplate(vendor.IsCountryTaxPayer),
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(transaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.ReservationNumber, reservation.ReservationId },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(GetDueDate(reservation.EndTime)) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(GetDueDate(reservation.EndTime)) },
                { CTransactionTemplateDataEnum.UserName, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerUserName, customer.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, customer.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, customer.Dic },
                { CTransactionTemplateDataEnum.PlaceOfBusiness, vendor.PlaceOfBusiness },
                { CTransactionTemplateDataEnum.Ico, vendor.Ico },
                { CTransactionTemplateDataEnum.Count, 1 },
                { CTransactionTemplateDataEnum.ProductDescription, description },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(Math.Abs(priceBase)) },
                { CTransactionTemplateDataEnum.Tax, PriceUtil.GetPriceString(Math.Abs(tax)) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(Math.Abs(transaction.Amount)) }
            }.Concat(couponData).ToDictionary(x => x.Key, x => x.Value)
        };
    }

    private async Task<ExportDataVM> ExportLessonCommission(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var reservation = await _appReservationRepository.GetRawDbEntity(transaction.ReservationId!.Value);
        var classDuration = await _cClassDurationRepository.Get(reservation.ClassDurationId);
        var tutor = await _appUserRepository.GetById(reservation.HostId);
        var student = await _appUserRepository.GetById(reservation.AttenderId);
        var description = await GetLessonCommissionDescription(
            student.GetUserName(),
            classDuration.ExpectedDuration
        );
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.PlatformsCommissionReceipt,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(transaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(GetDueDate(reservation.EndTime)) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(GetDueDate(reservation.EndTime)) },
                { CTransactionTemplateDataEnum.CustomerUserName, tutor.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, tutor.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, tutor.Dic },
                { CTransactionTemplateDataEnum.ReservationNumber, reservation.ReservationId },
                { CTransactionTemplateDataEnum.ProductDescription, description },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(Math.Abs(transaction.Amount)) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(Math.Abs(transaction.Amount)) },
            }
        };
    }

    private async Task<ExportDataVM> ExportPackageSaleCancellationCommission(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var package = await _appReservationPackageRepository.GetRawDbEntity(transaction.ReservationPackageId!.Value);
        var tutor = await _appUserRepository.GetById(package.HostId);
        var allReservations =
            await _appReservationRepository.GetAllRawByPackageId(transaction.ReservationPackageId!.Value);
        var completedReservations = allReservations
            .Where(x => AppReservationRepository.ConfirmedStates.Contains(x.ReservationStateId))
            .ToList();
        var description = await GetPackageCancellationFeeDescription(completedReservations.Count);
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.PlatformsCommissionReceipt,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(transaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.CustomerUserName, tutor.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, tutor.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, tutor.Dic },
                { CTransactionTemplateDataEnum.ProductDescription, description },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(Math.Abs(transaction.Amount)) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(Math.Abs(transaction.Amount)) },
            }
        };
    }

    private async Task<ExportDataVM> ExportRefund(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var sourceTransaction = await _appTransactionRepository.GetRawDbEntity(transaction.SourceTransactionId!.Value);
        var refundReason =
            await _cTransactionRefundReasonRepository.GetRawDbEntity(transaction.TransactionRefundReasonId!.Value);
        var reservation = await _appReservationRepository.GetRawDbEntity(transaction.ReservationId!.Value);
        var duration = await _cClassDurationRepository.Get(reservation.ClassDurationId);
        var vendor = await _appUserRepository.GetById(reservation.HostId);
        var customer = await GetCustomer(reservation.AttenderId);
        ;
        var refundDescription = await GetRefundDescription(
            vendor.GetUserName(),
            duration.ExpectedDuration,
            Math.Abs((int)Math.Round(transaction.Amount * 100 / reservation.Price))
        );
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.TutorsCreditMemo,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(sourceTransaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.ReservationNumber, reservation.ReservationId },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(reservation.EndTime) },
                { CTransactionTemplateDataEnum.UserName, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.PlaceOfBusiness, vendor.PlaceOfBusiness },
                { CTransactionTemplateDataEnum.Ico, vendor.Ico },
                { CTransactionTemplateDataEnum.CustomerUserName, customer.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, customer.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, customer.Dic },
                { CTransactionTemplateDataEnum.Count, 1 },
                { CTransactionTemplateDataEnum.ProductDescription, refundDescription },
                { CTransactionTemplateDataEnum.Reason, await GetTranslation(refundReason.NameKey) },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(-Math.Abs(transaction.Amount)) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(-Math.Abs(transaction.Amount)) },
            }
        };
    }

    private async Task<ExportDataVM> ExportRefundLessonCommissionCreditMemo(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        if (transaction.ReservationIssueId.HasValue)
        {
            return await ExportRefundLessonCommissionAfterIssue(transactionId, userId);
        }

        if (transaction.TransactionRefundReasonId == (int)CTransactionRefundReasonEnum.PackageCancellation)
        {
            return await ExportRefundLessonCommissionAfterCancellation(transactionId, userId);
        }

        return await ExportRefundLessonCommissionAfterReschedule(transactionId, userId);
    }

    private async Task<ExportDataVM> ExportRefundLessonCommissionAfterIssue(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var sourceTransaction = await _appTransactionRepository.GetRawDbEntity(transaction.SourceTransactionId!.Value);
        var refundReason =
            await _cTransactionRefundReasonRepository.GetRawDbEntity(transaction.TransactionRefundReasonId!.Value);
        var reservation = await _appReservationRepository.GetRawDbEntity(transaction.ReservationId!.Value);
        var tutorsMemo = await _appTransactionRepository.GetRawDbEntityByIssueIdAndType(
            transaction.ReservationIssueId!.Value,
            CTransactionTypeEnum.RefundCreditMemo,
            reservation.HostId
        );
        var vendor = await _appUserRepository.GetById(reservation.HostId);
        var customer = await GetCustomer(reservation.AttenderId);
        ;
        var duration = await _cClassDurationRepository.Get(reservation.ClassDurationId);
        var commissionDescription = await GetRefundLessonCommissionDescription(
            customer.GetUserName(),
            duration.ExpectedDuration,
            Math.Abs((int)Math.Round(tutorsMemo.Amount * 100 / reservation.Price))
        );
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.PlatformsCreditMemo,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(sourceTransaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.ReservationNumber, reservation.ReservationId },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.CustomerUserName, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, vendor.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, vendor.Dic },
                { CTransactionTemplateDataEnum.Count, 1 },
                { CTransactionTemplateDataEnum.ProductDescription, commissionDescription },
                { CTransactionTemplateDataEnum.Reason, await GetTranslation(refundReason.NameKey) },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(-transaction.Amount) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(-transaction.Amount) },
            }
        };
    }

    private async Task<ExportDataVM> ExportRefundLessonCommissionAfterCancellation(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var refundReason =
            await _cTransactionRefundReasonRepository.GetRawDbEntity(transaction.TransactionRefundReasonId!.Value);
        var package = await _appReservationPackageRepository.GetRawDbEntity(transaction.ReservationPackageId!.Value);
        var cancelledReservations = await _appReservationRepository.GetAllRawByPackageId(
            transaction.ReservationPackageId!.Value,
            CReservationStateEnum.CanceledByUser
        );
        var count = cancelledReservations.Any() ? cancelledReservations.Count : 1;
        var cancelledTransactions = await _appTransactionRepository.GetRawDbEntitiesByReservationIdsAndOwnerId(
            cancelledReservations.Select(x => x.ReservationId),
            package.HostId,
            CTransactionTypeEnum.LessonCommission
        );
        var vendor = await _appUserRepository.GetById(package.HostId);
        var customer = await GetCustomer(package.AttenderId);
        var duration = await _cClassDurationRepository.Get(package.ClassDurationId);
        var commissionDescription = await GetLessonCommissionDescription(
            customer.GetUserName(),
            duration.ExpectedDuration
        );
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.PlatformsCreditMemo,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                {
                    CTransactionTemplateDataEnum.Id,
                    GetRelatedIdsList(cancelledTransactions.Select(x => x.TransactionGroupId))
                },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.CustomerUserName, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, vendor.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, vendor.Dic },
                { CTransactionTemplateDataEnum.Count, count },
                { CTransactionTemplateDataEnum.ProductDescription, commissionDescription },
                { CTransactionTemplateDataEnum.Reason, await GetTranslation(refundReason.NameKey) },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(-transaction.Amount / count) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(-transaction.Amount) },
            }
        };
    }

    private async Task<ExportDataVM> ExportRefundLessonCommissionAfterReschedule(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var sourceTransaction = await _appTransactionRepository.GetRawDbEntity(transaction.SourceTransactionId!.Value);
        var refundReason =
            await _cTransactionRefundReasonRepository.GetRawDbEntity(transaction.TransactionRefundReasonId!.Value);
        var package = await _appReservationPackageRepository.GetRawDbEntity(transaction.ReservationPackageId!.Value);
        var vendor = await _appUserRepository.GetById(package.HostId);
        var customer = await GetCustomer(package.AttenderId);
        var duration = await _cClassDurationRepository.Get(package.ClassDurationId);
        var commissionDescription = await GetLessonCommissionDescription(
            customer.GetUserName(),
            duration.ExpectedDuration
        );
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.PlatformsCreditMemo,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(sourceTransaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.CustomerUserName, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, vendor.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, vendor.Dic },
                { CTransactionTemplateDataEnum.Count, 1 },
                { CTransactionTemplateDataEnum.ProductDescription, commissionDescription },
                { CTransactionTemplateDataEnum.Reason, await GetTranslation(refundReason.NameKey) },
                { CTransactionTemplateDataEnum.Price, PriceUtil.GetPriceString(-transaction.Amount) },
                { CTransactionTemplateDataEnum.TotalPrice, PriceUtil.GetPriceString(-transaction.Amount) },
            }
        };
    }


    private async Task<ExportDataVM> ExportWithdrawFee(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var sourceTransaction = await _appTransactionRepository.GetRawDbEntity(transaction.SourceTransactionId!.Value);
        return await ExportWithdrawTransaction(sourceTransaction.TransactionId, sourceTransaction.Created,
            sourceTransaction.Amount);
    }

    private async Task<ExportDataVM> ExportWithdrawTransaction(
        int transactionId,
        DateTime created,
        double withdrawAmountWithoutFee
    )
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var transactionState = await _cTransactionStateRepository.GetRawDbEntity(transaction.TransactionStateId);
        var wallet = await _appUserWalletRepository.GetRawDbEntity(transaction.UserWalletId);
        var user = await _appUserRepository.GetById(wallet.UserId);
        var feeDescription = await GetWithdrawFeeDescription();
        var withdrawDescription = await GetWithdrawDescription(Math.Abs(withdrawAmountWithoutFee));
        var stateTranslation = await GetTranslation(transactionState.NameKey);
        var commissionData = user.UserRoleId == (int)CUserRoleEnum.Student
            ? new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.FeeDescription, feeDescription },
                {
                    CTransactionTemplateDataEnum.FeeAmount,
                    PriceUtil.GetPriceString(PriceUtil.GetOurWithdrawPriceFee(-Math.Abs(withdrawAmountWithoutFee)))
                }
            }
            : new Dictionary<CTransactionTemplateDataEnum, object>();
        var totalAmount = user.UserRoleId == (int)CUserRoleEnum.Student
            ? PriceUtil.GetWithdrawAmountAfterFee(Math.Abs(withdrawAmountWithoutFee))
            : Math.Abs(withdrawAmountWithoutFee);
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.PlatformsWithdrawalConfirmation,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(transaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(created) },
                { CTransactionTemplateDataEnum.DueDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.ExecutionDate, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.State, stateTranslation },
                { CTransactionTemplateDataEnum.CustomerUserName, user.GetUserName() },
                { CTransactionTemplateDataEnum.CustomerIco, user.Ico },
                { CTransactionTemplateDataEnum.CustomerDic, user.Dic },
                { CTransactionTemplateDataEnum.WithdrawDescription, withdrawDescription },
                {
                    CTransactionTemplateDataEnum.WithdrawAmount,
                    PriceUtil.GetPriceString(Math.Abs(withdrawAmountWithoutFee))
                },
                { CTransactionTemplateDataEnum.TotalAmount, PriceUtil.GetPriceString(totalAmount) },
            }.Concat(commissionData).ToDictionary(x => x.Key, x => x.Value)
        };
    }

    private async Task<ExportDataVM> ExportMoneyTransfer(int transactionId, int userId)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var transactionState = await _cTransactionStateRepository.GetRawDbEntity(transaction.TransactionStateId);
        var vendor = await _appUserRepository.GetById(transaction.VendorId, onlyConfirmed: false, includeDeleted: true);
        var customer =
            await _appUserRepository.GetById(transaction.CustomerId, onlyConfirmed: false, includeDeleted: true);
        var stateTranslation = await GetTranslation(transactionState.NameKey);
        var transferDescription = await GetTranslation(TranslationConstants.MoneyTransferDescription);
        return new ExportDataVM
        {
            CTransactionTemplateTypeEnum = CTransactionTemplateTypeEnum.MoneyTransferConfirmation,
            Data = new Dictionary<CTransactionTemplateDataEnum, object>
            {
                { CTransactionTemplateDataEnum.Id, GetInvoiceNumber(transaction.TransactionGroupId) },
                { CTransactionTemplateDataEnum.State, stateTranslation },
                { CTransactionTemplateDataEnum.Created, GetDateFormatted(transaction.Created) },
                { CTransactionTemplateDataEnum.Sender, vendor.GetUserName() },
                { CTransactionTemplateDataEnum.Recipient, customer.GetUserName() },
                { CTransactionTemplateDataEnum.TransferDescription, transferDescription },
                { CTransactionTemplateDataEnum.TotalAmount, PriceUtil.GetPriceString(Math.Abs(transaction.Amount)) },
            }
        };
    }

    private string GetCsvFileName() => $"transactions_{_timeUtility.GetUtcNow():yyyy-MM-dd}";
    private static string GetInvoiceNumber(int transactionId) => $"Y-{transactionId}";
    private static string GetDateFormatted(DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");
    private static DateTime GetDueDate(DateTime dateTime) => dateTime.AddDays(7);

    private static string GetRelatedIdsList(IEnumerable<int> transactionIds) =>
        string.Join(", ", transactionIds.Select(GetInvoiceNumber));

    private async Task<AppTransactionPdfFileVM> ExportData(int transactionId, ExportDataVM data)
    {
        var transaction = await _appTransactionRepository.GetRawDbEntity(transactionId);
        var templateData =
            await _cTransactionTemplateRepository.GetTemplateData(data.CTransactionTemplateTypeEnum);
        var templateDataString = Encoding.UTF8.GetString(templateData);

        var template = Parser.Parse(templateDataString);
        var dictionary = data.Data.ToDictionary(x => x.Key.GetDescription(), x => x.Value);
        var content = template(dictionary);

        var memoryStream = new MemoryStream();
        using var converter = new Converter();
        converter.ConvertToPdf(content, memoryStream, new PageSettings());
        memoryStream.Position = 0;

        return new AppTransactionPdfFileVM
        {
            Name =
                $"{((CTransactionTypeEnum)transaction.TransactionTypeId).GetEnumKey()}_{transaction.Created:yyyy_MM_dd}.pdf",
            Stream = memoryStream
        };
    }

    private async Task ThrowIfNotBelongToUser(AppTransaction transaction, int userId)
    {
        var userWalletId = await _appUserWalletRepository.GetUserWalletId(userId);
        var wallet = await _appUserWalletRepository.GetRawDbEntity(transaction.UserWalletId);
        var transactionOwnerAppUser = await _appUserRepository.GetRawDbEntity(wallet.UserId, includeDeleted: true);
        if (transactionOwnerAppUser.InviterId == userId)
        {
            return;
        }

        if (transaction.UserWalletId != userWalletId)
        {
            throw new ForbiddenException();
        }
    }

    private static IReadOnlyDictionary<string, string> GetLocalizedCsvHeaders(
        IReadOnlyDictionary<string, string> translations)
    {
        const string prefix = TranslationConstants.CsvExportColumnsPrefix;
        var translationKeys = typeof(AppTransactionExportVM).GetProperties()
            .Select(x => (x.Name, x.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute))
            .Select(x => (x.Name, x.Item2?.Description ?? ""))
            .Select(x => (x.Name, $"{prefix}{x.Item2}".ToLower()))
            .ToList();

        return translationKeys
            .ToDictionary(x => x.Name, x => TranslationUtil.Translate(translations, x.Item2));
    }

    private async Task<string> GetLessonCommissionDescription(
        string userName,
        int duration
    )
    {
        const string translationKey = TranslationConstants.LessonCommissionProductName;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "userName", userName },
                { "duration", duration.ToString() }
            });
    }

    private async Task<string> GetLessonDescription(string tutorName, int duration)
    {
        const string translationKey = TranslationConstants.LessonProductName;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "userName", tutorName },
                { "duration", duration.ToString() }
            });
    }

    private async Task<string> GetRefundDescription(string tutorName, int duration, int percentage)
    {
        const string translationKey = TranslationConstants.RefundDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "userName", tutorName },
                { "duration", duration.ToString() },
                { "percentage", percentage.ToString() }
            });
    }

    private async Task<string> GetRefundLessonCommissionDescription(string studentName, int duration, int percentage)
    {
        const string translationKey = TranslationConstants.RefundCommissionDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "userName", studentName },
                { "duration", duration.ToString() },
                { "percentage", percentage.ToString() }
            });
    }

    private async Task<string> GetWithdrawDescription(double transactionAmount)
    {
        const string translationKey = TranslationConstants.WithdrawalDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "amount", PriceUtil.GetPriceString(transactionAmount) }
            });
    }

    private async Task<string> GetWithdrawFeeDescription()
    {
        const string translationKey = TranslationConstants.WithdrawalCommissionDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "commission", $"{CommonConstants.WithdrawCommissionRate * 100}%" }
            });
    }

    private async Task<string> GetPackageCancellationFeeDescription(int confirmedReservationsCount)
    {
        const string translationKey = TranslationConstants.PackageSaleCancellationDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "count", confirmedReservationsCount.ToString() }
            });
    }

    private async Task<string> GetCouponShortDescription(int discountPercentage)
    {
        const string translationKey = TranslationConstants.CouponShortDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "discount", discountPercentage.ToString() }
            });
    }

    private async Task<string> GetCouponDescription(int discountPercentage, string username)
    {
        const string translationKey = TranslationConstants.CouponDescription;
        return TranslationUtil.Translate(
            await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, translationKey),
            translationKey,
            new Dictionary<string, string>
            {
                { "discount", discountPercentage.ToString() },
                { "username", username }
            });
    }

    private async Task<string> GetTranslation(string key)
    {
        return TranslationUtil.Translate(await _cTranslationRepository.GetTranslationByKeyPrefix(LanguageId, key), key);
    }

    private async Task<AppUserBasicVM> GetCustomer(int studentId)
    {
        var student = await _appUserRepository.GetById(studentId, includeDeleted: true);
        var company = student.InviterId.HasValue
            ? await _appUserRepository.GetById(student.InviterId.Value)
            : null;
        return company ?? student;
    }

    private static CTransactionTemplateTypeEnum GetTutorsTemplate(bool isCountryTaxPayer)
    {
        return isCountryTaxPayer
            ? CTransactionTemplateTypeEnum.TutorsReceiptWithTaxes
            : CTransactionTemplateTypeEnum.TutorsReceiptWithoutTaxes;
    }

    private record ExportDataVM
    {
        public CTransactionTemplateTypeEnum CTransactionTemplateTypeEnum { get; set; }

        public IDictionary<CTransactionTemplateDataEnum, object> Data { get; set; } =
            new Dictionary<CTransactionTemplateDataEnum, object>();
    }
}