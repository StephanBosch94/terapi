using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;
using ULearn.Be.Models.ViewModels.AppReservationIssue;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class RefundApprovedNotificationVM : NotificationReservationIssueBaseVM
{
    public RefundApprovedNotificationVM(AppReservationIssueForNotificationVM reservationIssue) : base(reservationIssue)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public RefundApprovedNotificationVM()
    {
    }

    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.RefundApproved);
}