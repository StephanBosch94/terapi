using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CObjectStateSeeder
{
    public static readonly List<CObjectState> Entities = CodetableUtil
        .GetEnumValues<CObjectStateEnum>(CCodetablePrefixesEnum.CObjectState)
        .Select(
            x => new CObjectState()
            {
                ObjectStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}