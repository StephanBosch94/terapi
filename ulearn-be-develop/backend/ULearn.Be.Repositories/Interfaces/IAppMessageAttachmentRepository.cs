using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppMessage;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppMessageAttachmentRepository
{
    Task Add(int messageId, IEnumerable<MessageAttachmentInputVM> attachments);
    Task<AppMessageAttachment> GetByFilePath(string filePath);
}