namespace ULearn.Be.Common.Utils;

public static class FileUtil
{
    public const string MessageAttachmentDirectory = "message-attachment";
    public static string GetMessageAttachmentDirectory(int messageId) => $"message/{messageId}";
    public static string GetMessageAttachmentPath(int messageId, string fileName) => $"{GetMessageAttachmentDirectory(messageId)}/{fileName}";
}