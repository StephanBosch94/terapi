using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

namespace ULearn.Be.Models.InputModels;

public class AppNotificationIM
{
    public int RecipientId { get; set; }
    public int SenderId { get; set; }
    public CNotificationTypeEnum NotificationTypeEnum { get; set; }
    public NotificationBaseVM Data { get; set; }
}