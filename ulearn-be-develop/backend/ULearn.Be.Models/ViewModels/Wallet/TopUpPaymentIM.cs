using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.ViewModels.Wallet;

public record TopUpPaymentIM
{
    [Required] public double Amount { get; set; }
}