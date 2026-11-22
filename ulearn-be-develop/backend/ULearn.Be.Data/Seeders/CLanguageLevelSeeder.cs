using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CLanguageLevelSeeder
{
    public static readonly List<CLanguageLevel> Entities = CodetableUtil
        .GetEnumValues<CLanguageLevelEnum>(CCodetablePrefixesEnum.CLanguageLevel)
        .Select(
            x => new CLanguageLevel
            {
                LanguageLevelId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}