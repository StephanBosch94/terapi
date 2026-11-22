using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionForNotificationVM
{
    public int TransactionId { get; set; }
    public int TransactionTypeId { get; set; }
    public double Amount { get; set; }
    public AppUserCommonVM Creator { get; set; }
    public AppUserCommonVM Approver { get; set; }
}