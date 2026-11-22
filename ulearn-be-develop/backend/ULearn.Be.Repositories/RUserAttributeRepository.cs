using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.AppUser.Attribute;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserAttributeRepository : BaseRepository, IRUserAttributeRepository
{
    private readonly RepositoryContext _db;

    public RUserAttributeRepository(RepositoryContext db) : base(db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserAttributeVM>> GetByUserId(int userId)
    {
        return await Query()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.UserAttributeTypeId)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ILookup<int, UserAttributeVM>> GetAttributesByUserId(IEnumerable<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .ToApiList()
            .ToListAsync();

        return results.ToLookup(x => x.UserId);
    }

    private IQueryable<RUserAttribute> Query(bool asNoTracking = true)
    {
        var query = _db.RUserAttributes;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}