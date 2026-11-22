using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CTokenTypeSeeder
{
    public static readonly List<CTokenType> Entities = CodetableUtil
        .GetEnumValues<CTokenTypeEnum>(CCodetablePrefixesEnum.CTokenType)
        .Select(
            x => new CTokenType
            {
                TokenTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}