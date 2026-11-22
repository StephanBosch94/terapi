using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Utils;

public static class RoomStateEnumUtil
{
    public static CRoomStateEnum GetRoomState(CRoomStateEnum previousRoomState, IEnumerable<CRoomActivityTypeEnum> latestRoomOpponentActivities)
    {
        var lowestState = latestRoomOpponentActivities.MinBy(x => x);
        var lowestPossibleState = previousRoomState == CRoomStateEnum.Created
            ? CRoomStateEnum.Created
            : CRoomStateEnum.Started; 
        return lowestState switch
        {
            >= CRoomActivityTypeEnum.Start => CRoomStateEnum.Started,
            _ => lowestPossibleState
        };
    }
}