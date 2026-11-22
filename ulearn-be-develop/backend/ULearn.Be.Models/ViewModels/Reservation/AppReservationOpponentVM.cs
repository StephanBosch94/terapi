using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppReservationOpponentVM
{
    public int ReservationId { get; set; }
    public AppUserCommonVM Host { get; set; }
    public AppUserCommonVM Attender { get; set; }
}