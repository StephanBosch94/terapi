using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.ViewModels.Wallet;

public record TopUpUserPaymentIM : TopUpPaymentIM
{
    [Required] public int UserId { get; set; }
}