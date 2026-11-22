using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserFilePermissionRepository: BaseRepository, IRUserFilePermissionRepository
{
    private readonly RepositoryContext _db;

    public RUserFilePermissionRepository(RepositoryContext db) : base(db)
    {
        _db = db;
    }
    
    public async Task<IReadOnlyCollection<int>> GetUserIds(string filePath)
    {
        return await Query()
            .Where(x => x.FilePath == filePath)
            .Select(x => x.UserId)
            .ToListAsync();
    }

    public Task Add(IReadOnlyCollection<string> filePaths, IReadOnlyCollection<int> userIds)
    {
        var entities = filePaths
            .SelectMany(filePath => userIds.Select(userId => new RUserFilePermission
            {
                UserId = userId,
                FilePath = filePath
            }));
        
        _db.RUserFilePermissions.AddRange(entities);
        return _db.SaveChangesAsync();
    }

    private IQueryable<RUserFilePermission> Query(bool asNoTracking = true)
    {
        var query = _db.RUserFilePermissions;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}