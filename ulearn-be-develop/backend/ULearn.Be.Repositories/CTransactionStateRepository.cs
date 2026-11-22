using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTransactionStateRepository : BaseService, ICTransactionStateRepository
{
    private readonly RepositoryContext _db;

    public CTransactionStateRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<CTransactionState> GetRawDbEntity(int transactionStateId)
    {
        var entity = await Query()
            .Where(x => x.TransactionStateId == transactionStateId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(transactionStateId, entity);

        return entity;
    }

    private IQueryable<CTransactionState> Query(bool asNoTracking = true)
    {
        var query = _db.CTransactionStates;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}