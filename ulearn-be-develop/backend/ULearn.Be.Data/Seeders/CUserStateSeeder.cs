using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CUserStateSeeder
{
    private const CCodetablePrefixesEnum Prefix = CCodetablePrefixesEnum.CUserState;

    public static readonly List<CUserState> Entities = CodetableUtil
        .GetEnumValues<CUserStateEnum>(Prefix)
        .Select(
            x => new CUserState
            {
                UserStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
                CompanyAdminNameKey = $"{Prefix.GetEnumKey()}.{x.enumKey}.company_admin_name_key"
            }
        ).ToList();
}