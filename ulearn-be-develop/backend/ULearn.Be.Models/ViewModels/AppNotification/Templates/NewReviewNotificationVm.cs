using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class NewReviewNotificationVm : NotificationBaseVM
{
    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.NewReview);
    public int Rate { get; set; }
    public string Comment { get; set; }
}