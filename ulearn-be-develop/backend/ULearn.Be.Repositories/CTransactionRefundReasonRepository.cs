using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTransactionRefundReasonRepository: BaseService, ICTransactionRefundReasonRepository
{
    private readonly RepositoryContext _db;

    public CTransactionRefundReasonRepository(RepositoryContext db)
    {
        _db = db;
    }
    
    public async Task<CTransactionRefundReason> GetRawDbEntity(int transactionRefundReasonId)
    {
        var result = await Query()
            .Where(x => x.TransactionRefundReasonId == transactionRefundReasonId)
            .FirstOrDefaultAsync();
        
        ThrowIfNotFound(transactionRefundReasonId, result);

        return result;
    }
    
    private IQueryable<CTransactionRefundReason> Query(bool asNoTracking = true)
    {
        var query = _db.CTransactionRefundReasons;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}