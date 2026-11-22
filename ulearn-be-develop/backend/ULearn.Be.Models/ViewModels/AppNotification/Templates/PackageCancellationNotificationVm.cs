using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class PackageCancellationNotificationVm : NotificationBaseVM
{
    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.PackageCancellation);
    public int NumberOfLessons { get; set; }
}