namespace ULearn.Be.Models.ViewModels.AppMessage;

public class MessageInputVM
{
    public int OpponentId { get; set; }
    public string TextValue { get; set; }
    public IReadOnlyCollection<MessageAttachmentInputVM> Attachments { get; set; } =
        new List<MessageAttachmentInputVM>();
}