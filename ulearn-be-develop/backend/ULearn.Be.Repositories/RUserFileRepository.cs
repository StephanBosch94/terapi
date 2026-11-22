using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.File;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserFileRepository : BaseService, IRUserFileRepository
{
    private readonly RepositoryContext _db;

    public RUserFileRepository(RepositoryContext db)
    {
        _db = db;
    }

    public Task<bool> Exists(int userId, CFileTypeEnum fileTypeEnum)
    {
        return Query(false)
            .Where(x => x.FileTypeId == (int)fileTypeEnum)
            .AnyAsync(x => x.UserId == userId);
    }

    public Task Add(IEnumerable<UserFileInputVM> userFileInputVms, int userId)
    {
        var entities = userFileInputVms.Select(x => new RUserFile
        {
            UserId = userId,
            ContentType = x.ContentType,
            FilePath = x.FilePath,
            FileTypeId = (int)x.FileTypeId,
            FileName = x.FileName
        });
        _db.RUserFiles.AddRange(entities);
        return _db.SaveChangesAsync();
    }

    public Task AddWithContent(UserFileInputVM userFileInputVm, byte[] content, int userId)
    {
        var entity = new RUserFile
        {
            UserId = userId,
            ContentType = userFileInputVm.ContentType,
            FilePath = userFileInputVm.FilePath,
            FileTypeId = (int)userFileInputVm.FileTypeId,
            FileName = userFileInputVm.FileName,
            Content = content
        };
        _db.RUserFiles.Add(entity);
        return _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<RUserFile>> GetDbEntities(int userId, CFileTypeEnum fileTypeEnum)
    {
        return await Query(false)
            .Where(x => x.UserId == userId)
            .Where(x => x.FileTypeId == (int)fileTypeEnum)
            .ToListAsync();
    }

    public Task RemoveRange(IReadOnlyCollection<RUserFile> userFiles)
    {
        _db.RUserFiles.RemoveRange(userFiles);
        return _db.SaveChangesAsync();
    }

    public async Task<RUserFile> GetDbEntity(string filepath, int userId)
    {
        var result = await Query()
            .Where(x => x.UserId == userId)
            .Where(x => x.FilePath == filepath)
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Could not find a file with path [{filepath}]", result);

        return result;
    }

    public async Task<RUserFile> GetByFilePath(string filepath)
    {
        var result = await Query()
            .Where(x => x.FilePath == filepath)
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Could not find a file with path [{filepath}]", result);

        return result;
    }

    public Task<RUserFile> GetByFilePathOrDefault(string filepath)
    {
        return Query()
            .Where(x => x.FilePath == filepath)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<UserFileVM>> GetByUserId(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ILookup<int, UserFileVM>> GetFilesByUserId(IEnumerable<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.UserId);
    }

    private IQueryable<RUserFile> Query(bool asNoTracking = true)
    {
        var query = _db.RUserFiles;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}