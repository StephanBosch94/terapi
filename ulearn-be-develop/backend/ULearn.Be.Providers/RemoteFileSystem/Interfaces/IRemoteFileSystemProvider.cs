using ULearn.Be.Providers.RemoteFileSystem.Models;

namespace ULearn.Be.Providers.RemoteFileSystem.Interfaces;

public interface IRemoteFileSystemProvider
{
    Task<RemoteFile> GetFile(string path);
    Task SaveFile(Stream file, string directory, string fileName, string contentType = null);
    Task EditFile(string path, FileEditVM edit);
    Task DeleteFile(string path);
}