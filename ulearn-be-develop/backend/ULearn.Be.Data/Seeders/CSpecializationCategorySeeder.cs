using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CSpecializationCategorySeeder
{
    private const CCodetablePrefixesEnum Prefix = CCodetablePrefixesEnum.CSpecializationCategory;

    public static readonly List<CSpecializationCategory> Entities = CodetableUtil
        .GetEnumValues<CSpecializationCategoryEnum>(Prefix)
        .Select(
            x => new CSpecializationCategory
            {
                SpecializationCategoryId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}