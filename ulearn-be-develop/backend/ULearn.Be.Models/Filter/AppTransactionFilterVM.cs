using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class AppTransactionFilterVM
{
    [DefaultSortColumn(3, true)] public int? TransactionId { get; set; }
    public int? TransactionStateId { get; set; }
    [DefaultSortColumn(2, true)] public int? TransactionTypeId { get; set; }
    public int? ReservationId { get; set; }
    [DefaultSortColumn(1, true)] public DateTime? Created { get; set; }
}