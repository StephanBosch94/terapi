using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CMessageStateSeeder
{
    public static readonly List<CMessageState> Entities = CodetableUtil
        .GetEnumValues<CMessageStateEnum>(CCodetablePrefixesEnum.CMessageState)
        .Select(
            x => new CMessageState()
            {
                MessageStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
            }
        ).ToList();
}