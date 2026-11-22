using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.ReservationPackage;

public class AppReservationPackageOpponentVM
{
    public int ReservationPackageId { get; set; }
    public AppUserCommonVM Host { get; set; }
    public AppUserCommonVM Attender { get; set; }
}