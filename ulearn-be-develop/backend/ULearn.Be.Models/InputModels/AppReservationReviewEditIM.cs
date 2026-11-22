using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class AppReservationReviewEditIM : AppReservationReviewIM
{
    [Required] public int ReservationReviewId { get; set; }
}