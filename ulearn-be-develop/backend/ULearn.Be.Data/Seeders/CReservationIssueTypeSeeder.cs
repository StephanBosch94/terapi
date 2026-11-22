using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CReservationIssueTypeSeeder
{
    public static readonly List<CReservationIssueType> Entities = CodetableUtil
        .GetEnumValues<CReservationIssueTypeEnum>(CCodetablePrefixesEnum.CReservationIssueType)
        .Select(
            x => new CReservationIssueType
            {
                ReservationIssueTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
                HasDescription = true
            }
        ).ToList();
}