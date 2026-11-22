using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CClassDurationRepository : BaseService, ICClassDurationRepository
{
    private readonly RepositoryContext _db;

    public CClassDurationRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<CClassDurationVM> Get(int classDurationId)
    {
        var entity = await Query()
            .Where(x => x.ClassDurationId == classDurationId)
            .ToApiList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(classDurationId, entity);

        return entity;
    }

    private IQueryable<CClassDuration> Query(bool asNoTracking = true)
    {
        var query = _db.CClassDurations;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}