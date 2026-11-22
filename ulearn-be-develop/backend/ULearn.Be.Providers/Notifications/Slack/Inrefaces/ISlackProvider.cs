using ULearn.Be.Providers.Configuration.Models;
using ULearn.Be.Providers.Notifications.Slack.Enums;

namespace ULearn.Be.Providers.Notifications.Slack.Inrefaces;

public interface ISlackProvider
{
    Task SendNotification(
        SlackInputVM input,
        SlackChannelPriorityEnum priorityEnum = SlackChannelPriorityEnum.High,
        bool throwIfNotSuccess = false
    );
}