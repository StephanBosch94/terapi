using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CNotificationTypeSeeder
{
    private static readonly string Prefix = CCodetablePrefixesEnum.CNotificationType.GetEnumKey();
    public static readonly List<CNotificationType> Entities = CodetableUtil
        .GetEnumValues<CNotificationTypeEnum>(CCodetablePrefixesEnum.CNotificationType)
        .Select(
            x => new CNotificationType
            {
                NotificationTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
                TitleKey = $"{Prefix}.{((CNotificationTypeEnum) x.id).GetEnumKey()}.title",
                SettingTitleKey = $"{Prefix}.{((CNotificationTypeEnum) x.id).GetEnumKey()}.setting.title",
                SettingDescriptionKey = $"{Prefix}.{((CNotificationTypeEnum) x.id).GetEnumKey()}.setting.description",
            }
        ).ToList();
}