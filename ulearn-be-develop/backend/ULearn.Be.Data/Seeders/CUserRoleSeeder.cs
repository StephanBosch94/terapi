using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CUserRoleSeeder
{
    public static readonly List<CUserRole> Entities = CodetableUtil
        .GetEnumValues<CUserRoleEnum>(CCodetablePrefixesEnum.CUserRole)
        .Select(
            x => new CUserRole
            {
                UserRoleId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}