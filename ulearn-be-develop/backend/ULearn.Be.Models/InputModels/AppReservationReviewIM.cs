using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class AppReservationReviewIM
{
    [Required] public int ReservationId { get; set; }
    [Required] public int Value { get; set; }
    public string TextValue { get; set; }
}