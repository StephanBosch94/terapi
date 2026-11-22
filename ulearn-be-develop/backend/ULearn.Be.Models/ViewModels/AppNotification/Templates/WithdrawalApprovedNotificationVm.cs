using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class WithdrawalApprovedNotificationVm : NotificationBaseVM
{
    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.WithdrawalApproved);
}