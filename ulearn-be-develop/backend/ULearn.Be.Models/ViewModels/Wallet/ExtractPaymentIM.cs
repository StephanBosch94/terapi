using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.ViewModels.Wallet;

public class ExtractPaymentIM
{
    [Required] public int UserId { get; set; }
    [Required] public double Amount { get; set; }
}