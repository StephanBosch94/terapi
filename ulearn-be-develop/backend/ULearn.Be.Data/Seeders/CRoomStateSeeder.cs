using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CRoomStateSeeder
{
    public static readonly List<CRoomState> Entities = CodetableUtil
        .GetEnumValues<CRoomStateEnum>(CCodetablePrefixesEnum.CRoomState)
        .Select(
            x => new CRoomState
            {
                RoomStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}