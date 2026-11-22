using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.Room;

public class AppRoomVM
{
    public int RoomId { get; set; }
    public string RoomKey { get; set; }
    public int RoomStateId { get; set; }
    public DateTime ScheduledFromLocalTime { get; set; }
    public DateTime ScheduledToLocalTime { get; set; }
    public AppUserCommonVM Host { get; set; }
    public AppUserCommonVM Attender { get; set; }
    public string AccessToken { get; set; }
    public bool IsAlreadyJoined { get; set; }
}