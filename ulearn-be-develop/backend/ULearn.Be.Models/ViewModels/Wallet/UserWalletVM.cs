using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.ViewModels.Wallet;

public class UserWalletVM
{
    public int UserId { get; set; }
    public int UserWalletId { get; set; }
    public double Amount { get; set; }
    public double ScheduledAmount { get; set; }
    public double RevenueThisMonth { get; set; }
    public double RevenuePreviousMonth { get; set; }
    public double RevenueThisYear { get; set; }
    public double RevenuePreviousYear { get; set; }
    public double WithdrawThisMonth { get; set; }
    public double WithdrawThisYear { get; set; }
    public double TotalRevenue { get; set; }
    public CCurrencyVM Currency { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankCode { get; set; }
}