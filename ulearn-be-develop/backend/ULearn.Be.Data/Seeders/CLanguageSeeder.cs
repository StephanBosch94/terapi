using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CLanguageSeeder
{
    public static readonly List<CLanguage> Entities = CodetableUtil
        .GetEnumValues<CLanguageEnum>(CCodetablePrefixesEnum.CLanguage)
        .Select(
            x => new CLanguage
            {
                LanguageId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}