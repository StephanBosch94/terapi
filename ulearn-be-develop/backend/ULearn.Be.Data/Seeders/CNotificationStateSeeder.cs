using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CNotificationStateSeeder
{
    public static readonly List<CNotificationState> Entities = CodetableUtil
        .GetEnumValues<CNotificationStateEnum>(CCodetablePrefixesEnum.CNotificationState)
        .Select(
            x => new CNotificationState
            {
                NotificationStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
            }
        ).ToList();
}