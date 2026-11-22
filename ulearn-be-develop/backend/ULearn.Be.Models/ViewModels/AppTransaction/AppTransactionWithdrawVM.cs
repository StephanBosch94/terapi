using Newtonsoft.Json;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppReservationIssue;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionWithdrawVM : AppTransactionWithdrawFilterVM
{
    public int TransactionId { get; set; }
    public CTransactionStateVM State { get; set; }
    public AppUserCommonVM Creator { get; set; }
    public AppTransactionWalletVM Wallet { get; set; }
    public double RequestedAmount { get; set; }
    public double AmountAfterFee { get; set; }
    public double WithdrawFee { get; set; }
    public CCurrencyVM AmountCurrency { get; set; }
    public new DateTime Created { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankCode { get; set; }
    [JsonIgnore] public new int TransactionStateId { get; set; }
}