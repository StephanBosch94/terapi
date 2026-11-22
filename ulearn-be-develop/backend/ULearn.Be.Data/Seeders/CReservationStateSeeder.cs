using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CReservationStateSeeder
{
    public static readonly List<CReservationState> Entities = CodetableUtil
        .GetEnumValues<CReservationStateEnum>(CCodetablePrefixesEnum.CReservationState)
        .Select(
            x => new CReservationState
            {
                ReservationStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}