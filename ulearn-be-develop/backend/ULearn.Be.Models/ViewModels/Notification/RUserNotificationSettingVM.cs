namespace ULearn.Be.Models.ViewModels.Notification;

public class RUserNotificationSettingVM
{
    public int NotificationTypeId { get; set; }
    public string NotificationTypeSettingTitleKey { get; set; }
    public string NotificationTypeSettingDescriptionKey { get; set; }
    public bool Excluded { get; set; }
}