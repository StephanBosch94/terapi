using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserFileRepository
{
    Task<bool> Exists(int userId, CFileTypeEnum fileTypeEnum);
    Task Add(IEnumerable<UserFileInputVM> userFileInputVms, int userId);
    Task AddWithContent(UserFileInputVM userFileInputVm, byte[] content, int userId);
    Task<IReadOnlyCollection<RUserFile>> GetDbEntities(int userId, CFileTypeEnum fileTypeEnum);
    Task RemoveRange(IReadOnlyCollection<RUserFile> userFiles);
    Task<RUserFile> GetDbEntity(string filepath, int userId);
    Task<RUserFile> GetByFilePath(string filepath);
    Task<RUserFile> GetByFilePathOrDefault(string filepath);
    Task<IReadOnlyCollection<UserFileVM>> GetByUserId(int userId);
    Task<ILookup<int, UserFileVM>> GetFilesByUserId(IEnumerable<int> userIds);
}