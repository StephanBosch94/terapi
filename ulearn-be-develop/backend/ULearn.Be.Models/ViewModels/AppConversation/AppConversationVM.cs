using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppConversation;

public class AppConversationVM
{
    public int ConversationId { get; set; }
    public AppUserCommonVM Initiator { get; set; }
    public AppUserCommonVM Opponent { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public AppMessageVM LatestMessage { get; set; }
    public int CountOfUnreadMessages { get; set; }
}