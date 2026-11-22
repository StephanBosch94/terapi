using System.Text.Json.Serialization;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionVM : AppTransactionFilterVM
{
    public new int TransactionId { get; set; }
    public int? ReservationPackageId { get; set; }
    public CTransactionTypeVM TransactionType { get; set; }
    public CTransactionStateVM TransactionState { get; set; }
    public double Amount { get; set; }
    public CCurrencyVM Currency { get; set; }
    public new DateTime Created { get; set; }
    public AppUserCommonVM Opponent { get; set; }
    public AppTransactionReservationVM Reservation { get; set; }
    public string ReceiptUrl { get; set; }
    [JsonIgnore] public new int? TransactionTypeId { get; set; }
    [JsonIgnore] public new int? TransactionStateId { get; set; }
}