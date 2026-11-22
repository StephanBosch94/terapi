using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.ViewModels.Wallet;

public class WalletDepositIM
{
    [Required] public double Amount { get; set; }
    [Required] public string CurrencyCode { get; set; }
}