using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTransactionTemplateTypeRepository : BaseService, ICTransactionTemplateTypeRepository
{
    private readonly RepositoryContext _db;

    public CTransactionTemplateTypeRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<CTransactionTemplateTypeEnum> GetTransactionTemplateTypeEnum(string transactionTypeKey)
    {
        var result = await Query()
            .Where(x => x.EnumKey == transactionTypeKey)
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Could not find a transaction type with enumKey: [{transactionTypeKey}]", result);

        return (CTransactionTemplateTypeEnum)result.TransactionTemplateTypeId;
    }

    private IQueryable<CTransactionTemplateType> Query(bool asNoTracking = true)
    {
        var query = _db.CTransactionTemplateTypes;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}