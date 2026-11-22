using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.UserRoomStatistic;

public class UserRoomStatisticInputVM
{
    public int RoomId { get; set; }
    public CRoomStatisticTypeEnum RoomStatisticTypeId { get; set; }
    public double Value { get; set; }
    public string TextValue { get; set; }
}