using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CWeeklyGoalSeeder
{
    public static readonly List<CWeeklyGoal> Entities = CodetableUtil
        .GetEnumValues<CWeeklyGoalEnum>(CCodetablePrefixesEnum.CWeeklyGoal)
        .Select(
            x => new CWeeklyGoal
            {
                WeeklyGoalId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}