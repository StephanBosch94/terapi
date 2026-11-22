using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppTransactionService
{
    Task<PagedVM<AppTransactionVM>> Filter(FilterView<AppTransactionFilterVM> filter, ClaimsPrincipal user);
    Task<PagedVM<AppTransactionWithdrawVM>> WithdrawFilter(FilterView<AppTransactionWithdrawFilterVM> filter);
    Task CompleteTransaction(int transactionId, ClaimsPrincipal user);
    Task<AppTransactionPdfFileVM> GetFile(int transactionId, ClaimsPrincipal user);
    Task<AppTransactionCsvFileVM> Export(AppTransactionExportIM options, ClaimsPrincipal user);
}