using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.Filter;

public class AppNotificationFilterVM
{
    public int? NotificationStateId { get; set; }
    [DefaultSortColumn(1, true)] public DateTime Created { get; set; }
}