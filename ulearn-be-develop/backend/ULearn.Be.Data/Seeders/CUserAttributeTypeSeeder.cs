using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CUserAttributeTypeSeeder
{
    public static readonly List<CUserAttributeType> Entities = CodetableUtil
        .GetEnumValues<CUserAttributeTypeEnum>(CCodetablePrefixesEnum.CUserAttributeType)
        .Select(
            x => new CUserAttributeType
            {
                UserAttributeTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}