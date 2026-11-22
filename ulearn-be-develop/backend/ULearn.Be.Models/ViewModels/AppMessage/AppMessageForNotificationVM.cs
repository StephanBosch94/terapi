using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppConversation;

public class AppMessageForNotificationVM
{
    public int ConversationId { get; set; }
    public int MessageId { get; set; }
    public AppUserCommonVM Sender { get; set; }
    public AppUserCommonVM Recipient { get; set; }
}