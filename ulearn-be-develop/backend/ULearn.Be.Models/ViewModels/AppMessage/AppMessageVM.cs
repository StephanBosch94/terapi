using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;

namespace ULearn.Be.Models.ViewModels.AppMessage;

public class AppMessageVM: MessageFilterVM
{
    public int ConversationId { get; set; }
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public CMessageStateEnum MessageStateId { get; set; }
    public new DateTime Created { get; set; }
    public new DateTime Updated { get; set; }
    public IEnumerable<MessageAttachmentVM> Attachments { get; set; }
}