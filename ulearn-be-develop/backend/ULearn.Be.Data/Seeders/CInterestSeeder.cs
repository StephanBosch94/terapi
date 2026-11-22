using ULearn.Be.Common.Attributes;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public class CInterestSeeder
{
    private const CCodetablePrefixesEnum Prefix = CCodetablePrefixesEnum.CInterest;

    public static readonly List<CInterest> Entities = CodetableUtil
        .GetEnumValues<CInterestEnum>(Prefix)
        .Select(
            x => new CInterest
            {
                InterestId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
                IconName = ((CInterestEnum)x.id).GetAttribute<IconNameAttribute>().Icon,
            }
        ).ToList();
}