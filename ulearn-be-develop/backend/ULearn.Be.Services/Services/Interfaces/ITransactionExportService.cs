using System.Security.Claims;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ITransactionExportService
{
    Task<AppTransactionPdfFileVM> ExportToPdf(int transactionId, ClaimsPrincipal user);
    Task<AppTransactionCsvFileVM> ExportAllToCsv(AppTransactionExportIM options, ClaimsPrincipal user);
}