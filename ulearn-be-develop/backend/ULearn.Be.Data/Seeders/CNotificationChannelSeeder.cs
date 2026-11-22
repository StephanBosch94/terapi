using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CNotificationChannelSeeder
{
    public static readonly List<CNotificationChannel> Entities = CodetableUtil
        .GetEnumValues<CNotificationChannelEnum>(CCodetablePrefixesEnum.CNotificationChannel)
        .Select(
            x => new CNotificationChannel
            {
                NotificationChannelId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
            }
        ).ToList();
}