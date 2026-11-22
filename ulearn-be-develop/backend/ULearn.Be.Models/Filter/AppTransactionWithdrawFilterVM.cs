using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class AppTransactionWithdrawFilterVM
{
    [DefaultSortColumn(1)] public DateTime? Created { get; set; }
    [DefaultSortColumn(2)] public int? TransactionStateId { get; set; }
}