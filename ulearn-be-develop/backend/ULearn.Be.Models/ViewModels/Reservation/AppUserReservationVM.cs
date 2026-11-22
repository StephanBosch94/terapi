
using Newtonsoft.Json;

namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppUserReservationVM
{
    public int ReservationId { get; set; }
    public int ReservationStateId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [JsonIgnore] public int HostId { get; set; }
}