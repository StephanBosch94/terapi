using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CRoomStatisticTypeSeeder
{
    public static readonly List<CRoomStatisticType> Entities = CodetableUtil
        .GetEnumValues<CRoomStatisticTypeEnum>(CCodetablePrefixesEnum.CRoomStatisticType)
        .Select(
            x => new CRoomStatisticType
            {
                RoomStatisticTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}