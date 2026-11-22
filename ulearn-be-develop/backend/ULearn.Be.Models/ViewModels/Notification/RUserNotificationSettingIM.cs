using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.ViewModels.Notification;

public class RUserNotificationSettingIM
{
    [Required] public int NotificationTypeId { get; set; }
}