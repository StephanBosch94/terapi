using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionForExportVM
{
    public int TransactionId { get; set; }
    public int TransactionGroupId { get; set; }
    public double Amount { get; set; }
    public DateTime Created { get; set; }
    public CTransactionStateVM State { get; set; }
    public CTransactionTypeVM Type { get; set; }
    public AppTransactionReservationVM Reservation { get; set; }
    public AppUserCommonVM Customer { get; set; }
    public AppUserCommonVM Vendor { get; set; }
}