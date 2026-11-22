using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CTokenProviderSeeder
{
    public static readonly List<CTokenProvider> Entities = CodetableUtil
        .GetEnumValues<CTokenProviderEnum>(CCodetablePrefixesEnum.CTokenProvider)
        .Select(
            x => new CTokenProvider
            {
                TokenProviderId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}