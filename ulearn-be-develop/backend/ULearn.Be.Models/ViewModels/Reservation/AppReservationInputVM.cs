using System.ComponentModel.DataAnnotations;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppReservationInputVM
{
    [Required] public int TutorId { get; set; }
    [Required] public CClassDurationEnum DurationMinutes { get; set; }
    [Required] public ICollection<DateTime> FromDateTimes { get; set; } = Array.Empty<DateTime>();
    [Required] public string CurrencyCode { get; set; }
    public bool? UseWalletCredits { get; set; }
    public string CouponCode { get; set; }
}