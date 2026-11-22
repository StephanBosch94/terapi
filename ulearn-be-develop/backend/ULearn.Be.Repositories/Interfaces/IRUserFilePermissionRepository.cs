namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserFilePermissionRepository
{
    Task<IReadOnlyCollection<int>> GetUserIds(string filePath);
    Task Add(IReadOnlyCollection<string> filePaths, IReadOnlyCollection<int> userIds);
}