using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppTransactionRepository : BaseRepository, IAppTransactionRepository
{
    private static readonly IReadOnlyCollection<int> WithdrawTypeIds = new List<int>
    {
        (int)CTransactionTypeEnum.Withdrawal,
        (int)CTransactionTypeEnum.MoneyTransfer
    };

    private static readonly IReadOnlyCollection<int> CoOwnedExcludedTypeIds = new List<int>
    {
        (int)CTransactionTypeEnum.MoneyTransfer
    };

    private static readonly IReadOnlyCollection<CTransactionStateEnum> CompletedStates = new List<CTransactionStateEnum>
    {
        CTransactionStateEnum.Pending,
        CTransactionStateEnum.Completed,
        CTransactionStateEnum.Canceled
    };

    private static readonly IReadOnlyCollection<CTransactionStateEnum> ScheduledStates = new List<CTransactionStateEnum>
    {
        CTransactionStateEnum.Scheduled,
        CTransactionStateEnum.WaitingForReview
    };

    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppTransactionRepository(ITimeUtility timeUtility, RepositoryContext db) : base(db)
    {
        _timeUtility = timeUtility;
        _db = db;
    }

    public async Task<AppTransaction> GetRawDbEntity(int transactionId)
    {
        var result = await QueryAll()
            .Where(x => x.TransactionId == transactionId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(transactionId, result);

        return result;
    }

    public async Task<AppTransaction> GetRawDbEntityByIssueIdAndType(
        int reservationIssueId,
        CTransactionTypeEnum transactionTypeEnum,
        int? userId = null,
        bool throwIfNotFound = true
    )
    {
        var result = await QueryAll()
            .Where(x => x.ReservationIssueId == reservationIssueId)
            .Where(x => x.TransactionTypeId == (int)transactionTypeEnum)
            .Where(x => userId == null || x.UserWallet.UserId == userId)
            .FirstOrDefaultAsync();

        if (result == null && throwIfNotFound)
        {
            throw new NotFoundException(
                $"Could not find a transaction with reservationIssueId: {reservationIssueId} and transactionTypeId: {transactionTypeEnum}");
        }

        return result;
    }

    public async Task<IReadOnlyCollection<AppTransaction>> GetRawDbEntitiesByReservationId(int reservationId)
    {
        return await QueryAll()
            .Where(x => x.ReservationId == reservationId)
            .ToListAsync();
    }

    public async Task<AppTransaction> GetRawDbEntityByReservationIdIdAndType(
        int reservationId,
        CTransactionTypeEnum transactionTypeEnum,
        bool throwIfNotFound = true
    )
    {
        var result = await QueryAll()
            .Where(x => x.ReservationId == reservationId)
            .Where(x => x.TransactionTypeId == (int)transactionTypeEnum)
            .FirstOrDefaultAsync();

        if (throwIfNotFound && result == null)
        {
            throw new NotFoundException(
                $"Could not find a transaction with reservationId: {reservationId} and transactionTypeId: {transactionTypeEnum}");
        }

        return result;
    }

    public async Task<IReadOnlyCollection<AppTransaction>> GetRawDbEntitiesByReservationIdsAndOwnerId(
        IEnumerable<int> reservationIds,
        int ownerId,
        CTransactionTypeEnum? transactionTypeEnum = null
    )
    {
        return await QueryAll()
            .Where(x => x.ReservationId != null)
            .Where(x => transactionTypeEnum == null || x.TransactionTypeId == (int)transactionTypeEnum)
            .Where(x => reservationIds.Contains(x.ReservationId.Value))
            .Where(x => x.UserWallet.UserId == ownerId)
            .ToListAsync();
    }

    public async Task<int> Add(AppTransactionGroupIM input)
    {
        var transactionIds = await AddRange(
            new List<AppTransactionGroupIM>
            {
                input
            });

        return transactionIds.FirstOrDefault();
    }

    public async Task<int> Add(AppTransactionIM input, int groupId)
    {
        var now = _timeUtility.GetUtcNow();
        var entity = CreateTransaction(input, now, groupId);
        _db.AppTransactions.Add(entity);
        await _db.SaveChangesAsync();

        return entity.TransactionId;
    }

    public async Task<IReadOnlyCollection<int>> AddRange(IEnumerable<AppTransactionGroupIM> inputVms)
    {
        var now = _timeUtility.GetUtcNow();
        var transactionGroups = inputVms.Select(x =>
            new AppTransactionGroup
            {
                Created = now,
                AppTransactions = x.Transactions.Select(y => CreateTransaction(y, now)).ToList()
            }).ToList();
        _db.AppTransactionGroups.AddRange(transactionGroups);
        await _db.SaveChangesAsync();

        return transactionGroups
            .SelectMany(x => x.AppTransactions)
            .Select(x => x.TransactionId)
            .ToHashSet();
    }

    public Task<double> GetCurrentBalance(int userId)
    {
        return GetAmountSum(userId, CompletedStates);
    }

    public Task<IReadOnlyDictionary<int, double>> GetScheduledBalanceByUserId(IReadOnlyCollection<int> userIds)
    {
        return GetAmountSumByUserId(userIds, ScheduledStates);
    }

    public Task<IReadOnlyDictionary<int, double>> GetTotalRevenueByUserId(IReadOnlyCollection<int> userIds)
    {
        return GetAmountSumByUserId(userIds, CompletedStates, WithdrawTypeIds);
    }

    public Task<double> GetRevenueThisMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var start = new DateTime(now.Year, now.Month, 1).ToUniversalTime();

        return GetSum(
            userId,
            start,
            start.AddMonths(1),
            excludeTransactionTypes: WithdrawTypeIds
        );
    }

    public Task<double> GetRevenuePreviousMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var start = new DateTime(now.Year, now.Month, 1).ToUniversalTime().AddMonths(-1);
        var end = new DateTime(now.Year, now.Month, 1).ToUniversalTime();

        return GetSum(
            userId,
            start,
            end,
            excludeTransactionTypes: WithdrawTypeIds
        );
    }

    public Task<double> GetRevenueThisYear(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var start = new DateTime(now.Year, 1, 1).ToUniversalTime();

        return GetSum(
            userId,
            start,
            start.AddYears(1),
            excludeTransactionTypes: WithdrawTypeIds
        );
    }

    public Task<double> GetRevenuePreviousYear(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var start = new DateTime(now.Year, 1, 1).ToUniversalTime().AddYears(-1);

        return GetSum(
            userId,
            start,
            start.AddYears(1),
            excludeTransactionTypes: WithdrawTypeIds
        );
    }

    public Task<double> GetWithdrawThisMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var start = new DateTime(now.Year, now.Month, 1).ToUniversalTime();

        return GetSum(
            userId,
            start,
            start.AddMonths(1),
            includeTransactionTypes: WithdrawTypeIds
        );
    }

    public Task<double> GetWithdrawThisYear(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var monthStart = new DateTime(now.Year, 1, 1).ToUniversalTime();

        return GetSum(
            userId,
            monthStart,
            monthStart.AddYears(1),
            includeTransactionTypes: WithdrawTypeIds
        );
    }

    public async Task<int> UpdateStateByReservationId(
        IEnumerable<int> reservationId,
        CTransactionStateEnum stateEnum,
        int userId,
        IEnumerable<CTransactionTypeEnum> transactionTypeEnums = null)
    {
        var now = _timeUtility.GetUtcNow();
        var transactionTypeFilter = transactionTypeEnums?.Select(x => (int)x);
        var transactions = await QueryAll(false)
            .Where(x => x.ReservationId != null)
            .Where(x => reservationId.Contains(x.ReservationId.Value))
            .Where(x => transactionTypeFilter == null || transactionTypeFilter.Contains(x.TransactionTypeId))
            .ToListAsync();

        foreach (var appTransaction in transactions)
        {
            appTransaction.UpdaterId = userId;
            appTransaction.Updated = now;
            appTransaction.TransactionStateId = (int)stateEnum;
            if (stateEnum == CTransactionStateEnum.Canceled)
            {
                appTransaction.PlannedCompletionTime = now;
            }
        }

        return await _db.SaveChangesAsync();
    }

    public Task<int> UpdateStateByReservationId(
        int reservationId,
        CTransactionStateEnum stateEnum,
        int userId,
        IEnumerable<CTransactionTypeEnum> transactionTypeEnums = null
    )
    {
        return UpdateStateByReservationId(new List<int> { reservationId }, stateEnum, userId, transactionTypeEnums);
    }

    public async Task<AppTransactionForNotificationVM> GetNotificationVmById(int transactionId)
    {
        var transaction = await QueryAll()
            .Where(x => x.TransactionId == transactionId)
            .ToAppTransactionForNotificationVmList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(transactionId, transaction);

        return transaction;
    }

    public async Task<ICollection<AppTransactionForExportVM>> GetExportData(AppTransactionExportIM options, int userId)
    {
        var taxOnly = options.TaxOnly == true;

        var owned = await GetTransactionsForExport(
            QueryAll().Where(x => x.UserWallet.UserId == userId),
            taxOnly
        );

        var coOwned = await GetTransactionsForExport(
            QueryAll()
                .Where(x => x.UserWallet.User.InviterId == userId)
                .Where(x => !CoOwnedExcludedTypeIds.Contains(x.TransactionTypeId)),
            taxOnly
        );

        return owned.Concat(coOwned)
            .OrderBy(x => x.TransactionId)
            .ToList();
    }

    public async Task<PagedVM<AppTransactionWithdrawVM>> WithdrawFilter(
        FilterView<AppTransactionWithdrawFilterVM> filter
    )
    {
        var results = await QueryAll()
            .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.Withdrawal)
            .ToWithdrawList()
            .ToPagedVMAsync(filter);

        EnhanceWithFees(results.Data);

        return results;
    }

    public Task<IReadOnlyDictionary<int, double>> GetCurrentBalanceByUserId(IReadOnlyCollection<int> userIds)
    {
        return GetAmountSumByUserId(userIds, CompletedStates);
    }

    public async Task Complete(int transactionId, int userId)
    {
        var transaction = await QueryAll(false)
            .Where(x => x.TransactionId == transactionId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(transactionId, transaction);
        var now = _timeUtility.GetUtcNow();
        transaction!.UpdaterId = userId;
        transaction.ApproverId = userId;
        transaction.Updated = now;
        transaction.Approved = now;
        transaction.TransactionStateId = (int)CTransactionStateEnum.Completed;

        await _db.SaveChangesAsync();
    }

    public Task<PagedVM<AppTransactionVM>> Filter(FilterView<AppTransactionFilterVM> filter, int userId)
    {
        var query = QueryOwnedAndCoOwned(userId);

        if (filter.Filter?.TransactionStateId != null)
        {
            query = query.Where(x => x.TransactionStateId == filter.Filter.TransactionStateId);
        }

        if (filter.Filter?.TransactionTypeId != null)
        {
            query = query.Where(x => x.TransactionTypeId == filter.Filter.TransactionTypeId);
        }

        return query
            .ToApiList()
            .ToPagedVMAsync(filter);
    }

    private static async Task<IReadOnlyCollection<AppTransactionForExportVM>> GetTransactionsForExport(
        IQueryable<AppTransaction> query,
        bool taxOnly
    )
    {
        var stateIds = CompletedStates.Select(x => (int)x);
        return await query
            .Where(x => !taxOnly || stateIds.Contains(x.TransactionStateId))
            .Where(x => !taxOnly || !WithdrawTypeIds.Contains(x.TransactionTypeId))
            .ToAppTransactionExportList()
            .OrderBy(x => x.TransactionId)
            .ToListAsync();
    }

    private AppTransaction CreateTransaction(AppTransactionIM dataItem, DateTime? now = null, int groupId = 0)
    {
        var nowFallback = _timeUtility.GetUtcNow();
        return new AppTransaction
        {
            SourceTransactionId = dataItem.SourceTransactionId,
            UserWalletId = dataItem.OwnerWalletId,
            VendorId = dataItem.VendorId,
            CustomerId = dataItem.CustomerId,
            TransactionTypeId = (int)dataItem.TransactionTypeEnum,
            TransactionStateId = (int)dataItem.TransactionStateEnum,
            PaymentServiceProviderId = (int)CPaymentServiceProviderEnum.Stripe,
            ReservationId = dataItem.ReservationId,
            ReservationPackageId = dataItem.ReservationPackageId,
            ReservationIssueId = dataItem.ReservationIssueId,
            PlannedCompletionTime = dataItem.PlannedCompletionTime,
            Amount = dataItem.Amount,
            SessionId = dataItem.SessionId,
            BankCode = dataItem.BankCode,
            BankAccountNumber = dataItem.BankAccountNumber,
            CreatorId = dataItem.CreatorId,
            UpdaterId = dataItem.CreatorId,
            Created = now ?? nowFallback,
            Updated = now ?? nowFallback,
            CurrencyId = (int)CCurrencyEnum.Czk,
            ReceiptUrl = dataItem.ReceiptUrl,
            TransactionGroupId = groupId,
            TransactionRefundReasonId = (int?)dataItem.TransactionRefundReasonEnum,
            CouponId = dataItem.CouponId,
            CouponSaleAmount = dataItem.CouponSaleAmount
        };
    }

    private static void EnhanceWithFees(IEnumerable<AppTransactionWithdrawVM> results)
    {
        foreach (var appTransactionWithdrawVm in results)
        {
            appTransactionWithdrawVm.WithdrawFee =
                appTransactionWithdrawVm.Creator.UserRoleId == (int)CUserRoleEnum.Student
                    ? PriceUtil.GetOurWithdrawPriceFee(appTransactionWithdrawVm.RequestedAmount)
                    : 0;
            appTransactionWithdrawVm.AmountAfterFee =
                appTransactionWithdrawVm.Creator.UserRoleId == (int)CUserRoleEnum.Student
                    ? PriceUtil.GetWithdrawAmountAfterFee(appTransactionWithdrawVm.RequestedAmount)
                    : appTransactionWithdrawVm.RequestedAmount;
        }
    }

    private Task<double> GetSum(
        int userId,
        DateTime startTime,
        DateTime endTime,
        IEnumerable<int> excludeTransactionTypes = null,
        IEnumerable<int> includeTransactionTypes = null
    )
    {
        return QueryCompleted()
            .Where(x => x.UserWallet.UserId == userId)
            .Where(x => excludeTransactionTypes == null || !excludeTransactionTypes.Contains(x.TransactionTypeId))
            .Where(x => includeTransactionTypes == null || includeTransactionTypes.Contains(x.TransactionTypeId))
            .Where(x => x.PlannedCompletionTime > startTime)
            .Where(x => x.PlannedCompletionTime < endTime)
            .SumAsync(x => x.Amount);
    }

    private Task<double> GetAmountSum(int userId, IEnumerable<CTransactionStateEnum> states)
    {
        var stateIds = states.Select(x => (int)x).ToHashSet();

        return QueryAll()
            .Where(x => x.UserWallet.UserId == userId)
            .Where(x => stateIds.Contains(x.TransactionStateId))
            .SumAsync(x => x.Amount);
    }

    private async Task<IReadOnlyDictionary<int, double>> GetAmountSumByUserId(
        IReadOnlyCollection<int> userIds,
        IEnumerable<CTransactionStateEnum> stateEnums,
        IEnumerable<int> excludedTypeIds = null
    )
    {
        var stateIds = stateEnums.Select(x => (int)x).ToHashSet();

        var results = await QueryAll()
            .Where(x => excludedTypeIds == null || !excludedTypeIds.Contains(x.TransactionTypeId))
            .Where(x => stateIds.Contains(x.TransactionStateId))
            .Where(x => userIds.Contains(x.UserWallet.UserId))
            .GroupBy(x => x.UserWallet.UserId)
            .Select(x => new
            {
                UserId = x.Key,
                Amount = x.Sum(y => y.Amount)
            })
            .ToDictionaryAsync(
                x => x.UserId,
                x => x.Amount
            );

        return userIds
            .ToDictionary(
                userId => userId,
                userId => results.TryGetValue(userId, out var result) ? result : 0
            );
    }

    private IQueryable<AppTransaction> QueryOwnedAndCoOwned(int userId)
    {
        var ownedQuery = QueryAll()
            .Where(x => x.UserWallet.UserId == userId);

        var colleaguesQuery = QueryAll()
            .Where(x => x.UserWallet.User.InviterId == userId)
            .Where(x => !CoOwnedExcludedTypeIds.Contains(x.TransactionTypeId));

        return ownedQuery.Concat(colleaguesQuery);
    }

    private IQueryable<AppTransaction> QueryCompleted(bool asNoTracking = true)
    {
        var completedStateIds = CompletedStates.Select(x => (int)x).ToHashSet();
        return QueryAll(asNoTracking)
            .Where(x => completedStateIds.Contains(x.TransactionStateId));
    }

    private IQueryable<AppTransaction> QueryAll(bool asNoTracking = true)
    {
        var query = _db.AppTransactions;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}