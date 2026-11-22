using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.Activity;

public class ActivityInputVM
{
    public CRoomActivityTypeEnum ActivityTypeId { get; set; }
    public string MessagePayload { get; set; }
}  