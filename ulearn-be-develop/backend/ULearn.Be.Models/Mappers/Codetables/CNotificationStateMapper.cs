using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CNotificationStateMapper
{
    public static CNotificationStateVM ToVm(CNotificationState dataItem)
    {
        return new CNotificationStateVM
        {
            NotificationStateId = dataItem.NotificationStateId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }
}