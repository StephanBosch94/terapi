using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CReservationPackageStateSeeder
{
    public static readonly List<CReservationPackageState> Entities = CodetableUtil
        .GetEnumValues<CReservationPackageStateEnum>(CCodetablePrefixesEnum.CReservationPackageState)
        .Select(
            x => new CReservationPackageState
            {
                ReservationStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}