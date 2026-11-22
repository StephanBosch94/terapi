using System.Text.Json.Serialization;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppReservationRoomVM
{
    public int RoomId { get; set; }
    public string RoomKey { get; set; }
    [JsonIgnore] public int HostId { get; set; }
    [JsonIgnore] public int AttenderId { get; set; }
}