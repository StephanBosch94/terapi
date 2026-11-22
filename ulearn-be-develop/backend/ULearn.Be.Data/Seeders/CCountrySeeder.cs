using ULearn.Be.Common.Attributes;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CCountrySeeder
{
    public static readonly List<CCountry> Entities = CodetableUtil
        .GetEnumValues<CCountryEnum>(CCodetablePrefixesEnum.CCountry)
        .Select(
            x => new CCountry
            {
                CountryId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
                CountryCode = ((CCountryEnum)x.id).GetAttribute<CountryCodeAttribute>().CountryCode
            }
        ).ToList();
}