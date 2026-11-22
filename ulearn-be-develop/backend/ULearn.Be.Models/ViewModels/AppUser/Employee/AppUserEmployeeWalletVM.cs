using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.AppUser.Employee;

public class AppUserEmployeeWalletVM
{
    public int WalletId { get; set; }
    public int UserId { get; set; }
    public double Amount { get; set; }
    public CCurrencyVM Currency { get; set; }
    public DateTime? LastMoneyTransferDate { get; set; }
}