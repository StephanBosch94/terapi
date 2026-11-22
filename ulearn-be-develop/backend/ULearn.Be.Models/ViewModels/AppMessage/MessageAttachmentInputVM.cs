using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Models.ViewModels.AppMessage;

public class MessageAttachmentInputVM: FileInputBaseVM
{
    public byte[] Content { get; set; }
}