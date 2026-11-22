using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CRoomActivitySeeder
{
    public static readonly List<CRoomActivityType> Entities = CodetableUtil
        .GetEnumValues<CRoomActivityTypeEnum>(CCodetablePrefixesEnum.CRoomActivity)
        .Select(
            x => new CRoomActivityType
            {
                RoomActivityTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}