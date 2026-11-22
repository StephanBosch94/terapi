using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;
using ULearn.Be.Models.ViewModels.AppReservationIssue;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class RefundRequestedNotificationVM : NotificationReservationIssueBaseVM
{
    public RefundRequestedNotificationVM(AppReservationIssueForNotificationVM entity) : base(entity)
    {
    }

    public RefundRequestedNotificationVM()
    {
    }

    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.RefundRequested);
}