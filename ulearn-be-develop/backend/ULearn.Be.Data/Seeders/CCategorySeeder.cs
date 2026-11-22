using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CCategorySeeder
{
    public static readonly List<CCategory> Entities = CodetableUtil
        .GetEnumValues<CCategoryEnum>(CCodetablePrefixesEnum.CCategory)
        .Select(
            x => new CCategory
            {
                CategoryId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}