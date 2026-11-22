namespace ULearn.Be.Models.ViewModels.AppMessage;

public class MessageInConversationInputVM
{
    public int ConversationId { get; set; }
    public string TextValue { get; set; }
    public IReadOnlyCollection<MessageAttachmentInputVM> Attachments { get; set; } =
        new List<MessageAttachmentInputVM>();
}