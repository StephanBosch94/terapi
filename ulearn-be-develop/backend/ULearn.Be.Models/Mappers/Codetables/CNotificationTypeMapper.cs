using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CNotificationTypeMapper
{
    public static CNotificationTypeVM ToVm(CNotificationType dataItem)
    {
        return new CNotificationTypeVM
        {
            NotificationTypeId = dataItem.NotificationTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            TitleKey = dataItem.TitleKey
        };
    }
}