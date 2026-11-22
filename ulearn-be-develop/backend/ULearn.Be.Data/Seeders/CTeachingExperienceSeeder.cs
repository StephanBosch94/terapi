using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CTeachingExperienceSeeder
{
    public static readonly List<CTeachingExperience> Entities = CodetableUtil
        .GetEnumValues<CTeachingExperienceEnum>(CCodetablePrefixesEnum.CTeachingExperience)
        .Select(
            x => new CTeachingExperience
            {
                TeachingExperienceId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}