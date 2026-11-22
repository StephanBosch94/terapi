using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class TutorProfileVisitedNotificationVm : NotificationBaseVM
{
    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.TutorProfileVisited);
    public string UserName { get; set; }
    public string Url { get; set; }
}