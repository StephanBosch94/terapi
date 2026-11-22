using ULearn.Be.Common.Models;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppTransactionRepository
{
    Task<AppTransaction> GetRawDbEntity(int transactionId);

    Task<AppTransaction> GetRawDbEntityByIssueIdAndType(
        int reservationIssueId,
        CTransactionTypeEnum transactionTypeEnum,
        int? userId = null,
        bool throwIfNotFound = true
    );

    Task<IReadOnlyCollection<AppTransaction>> GetRawDbEntitiesByReservationId(int reservationId);

    Task<AppTransaction> GetRawDbEntityByReservationIdIdAndType(
        int reservationId,
        CTransactionTypeEnum transactionTypeEnum,
        bool throwIfNotFound = true
    );

    Task<IReadOnlyCollection<AppTransaction>> GetRawDbEntitiesByReservationIdsAndOwnerId(
        IEnumerable<int> reservationIds,
        int ownerId,
        CTransactionTypeEnum? transactionTypeEnum = null
    );

    Task<int> Add(AppTransactionGroupIM input);
    Task<int> Add(AppTransactionIM input, int groupId);
    Task<IReadOnlyCollection<int>> AddRange(IEnumerable<AppTransactionGroupIM> inputVms);
    Task<double> GetCurrentBalance(int userId);
    Task<double> GetRevenueThisMonth(int userId);
    Task<double> GetRevenuePreviousMonth(int userId);
    Task<double> GetRevenueThisYear(int userId);
    Task<double> GetRevenuePreviousYear(int userId);
    Task<double> GetWithdrawThisMonth(int userId);
    Task<double> GetWithdrawThisYear(int userId);
    Task Complete(int transactionId, int userId);
    Task<PagedVM<AppTransactionVM>> Filter(FilterView<AppTransactionFilterVM> filter, int userId);
    Task<PagedVM<AppTransactionWithdrawVM>> WithdrawFilter(FilterView<AppTransactionWithdrawFilterVM> filter);
    Task<IReadOnlyDictionary<int, double>> GetCurrentBalanceByUserId(IReadOnlyCollection<int> userIds);
    Task<IReadOnlyDictionary<int, double>> GetScheduledBalanceByUserId(IReadOnlyCollection<int> userIds);
    Task<IReadOnlyDictionary<int, double>> GetTotalRevenueByUserId(IReadOnlyCollection<int> userIds);

    Task<int> UpdateStateByReservationId(int reservationId, CTransactionStateEnum stateEnum, int userId,
        IEnumerable<CTransactionTypeEnum> transactionTypeEnums = null);

    Task<int> UpdateStateByReservationId(
        IEnumerable<int> reservationId,
        CTransactionStateEnum stateEnum,
        int userId,
        IEnumerable<CTransactionTypeEnum> transactionTypeEnums = null
    );

    Task<AppTransactionForNotificationVM> GetNotificationVmById(int transactionId);
    Task<ICollection<AppTransactionForExportVM>> GetExportData(AppTransactionExportIM options, int userId);
}