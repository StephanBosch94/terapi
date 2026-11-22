using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CAuthenticationMethodSeeder
{
    public static readonly List<CAuthenticationMethod> Entities = CodetableUtil
        .GetEnumValues<CAuthenticationMethodEnum>(CCodetablePrefixesEnum.AuthenticationMethod)
        .Select(
            x => new CAuthenticationMethod
            {
                AuthenticationMethodId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}