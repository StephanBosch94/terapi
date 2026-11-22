using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CCouponTypeSeeder
{
    public static readonly List<CCouponType> Entities = CodetableUtil
        .GetEnumValues<CCouponTypeEnum>(CCodetablePrefixesEnum.CCouponType)
        .Select(
            x => new CCouponType()
            {
                CouponTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
            }
        ).ToList();
}