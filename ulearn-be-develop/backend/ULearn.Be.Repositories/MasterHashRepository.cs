using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class MasterHashRepository : BaseRepository, IMasterHashRepository
{
    private readonly RepositoryContext _db;
    private readonly IHashUtility _hashUtility;
    private readonly ITimeUtility _timeUtility;

    public MasterHashRepository(RepositoryContext db, IHashUtility hashUtility, ITimeUtility timeUtility) : base(db)
    {
        _db = db;
        _hashUtility = hashUtility;
        _timeUtility = timeUtility;
    }

    public async Task<ICollection<CMasterHash>> GetByEnumKeyPrefix(string enumKeyPrefix)
    {
        return await Query()
            .Where(x => x.EnumKey.Contains(enumKeyPrefix))
            .ToListAsync();
    }

    public Task<DateTime> GetLastUpdate(CMasterHashEnum masterHash)
    {
        return Query()
            .Where(x => x.MasterHashId == (int)masterHash)
            .Select(x => x.Updated)
            .FirstOrDefaultAsync();
    }

    public async Task<int> Update(ICollection<string> enumKeys, Guid? newGuid = null)
    {
        var dbEntities = await Query(false)
            .Where(x => enumKeys.Contains(x.EnumKey))
            .ToListAsync();

        foreach (var cMasterHash in dbEntities)
        {
            cMasterHash.Hash = newGuid ?? _hashUtility.GenerateGuid();
            cMasterHash.Updated = _timeUtility.GetUtcNow();
        }

        return await _db.SaveChangesAsync();
    }

    private IQueryable<CMasterHash> Query(bool asNoTracking = true)
    {
        var query = _db.CMasterHashes;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}