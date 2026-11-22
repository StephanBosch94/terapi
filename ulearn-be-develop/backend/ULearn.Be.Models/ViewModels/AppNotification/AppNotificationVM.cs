using Newtonsoft.Json;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppNotification;

public class AppNotificationVM : AppNotificationFilterVM
{
    public int NotificationId { get; set; }
    public int RecipientId { get; set; }
    public CNotificationStateVM NotificationState { get; set; }
    public CNotificationTypeVM NotificationType { get; set; }
    public AppUserCommonVM Sender { get; set; }
    public new DateTime Created { get; set; }
    public DateTime? ReadDateTime { get; set; }
    public NotificationBaseVM Data { get; set; }
    [JsonIgnore] public new int? NotificationStateId { get; set; }
    [JsonIgnore] public string JsonData { get; set; }
}