using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionWalletVM
{
    public double Amount { get; set; }
    public CCurrencyVM Currency { get; set; }
}