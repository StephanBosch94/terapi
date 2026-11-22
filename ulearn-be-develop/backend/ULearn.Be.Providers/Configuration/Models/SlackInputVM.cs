using ULearn.Be.Data.Enums;

namespace ULearn.Be.Providers.Configuration.Models;

public class SlackInputVM
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Text { get; set; }
    public int RoleId { get; set; }
    public CNotificationTypeEnum NotificationTypeEnum { get; set; }
}