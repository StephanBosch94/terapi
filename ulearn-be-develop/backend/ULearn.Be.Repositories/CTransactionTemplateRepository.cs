using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CTransactionTemplateRepository : BaseService, ICTransactionTemplateRepository
{
    private readonly RepositoryContext _db;

    public CTransactionTemplateRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task Upsert(Stream stream, string fileContentType,
        CTransactionTemplateTypeEnum transactionTemplateTypeEnum)
    {
        var entity = await Query(false)
            .Where(x => x.TransactionTemplateTypeId == (int)transactionTemplateTypeEnum)
            .FirstOrDefaultAsync();

        if (entity != null)
        {
            entity.ContentType = fileContentType;
            entity.Data = stream.ReadAllBytes();
            await _db.SaveChangesAsync();
            return;
        }

        _db.CTransactionTemplates.Add(
            new CTransactionTemplate
            {
                TransactionTemplateTypeId = (int)transactionTemplateTypeEnum,
                ContentType = fileContentType,
                Data = stream.ReadAllBytes()
            });

        await _db.SaveChangesAsync();
    }

    public async Task<byte[]> GetTemplateData(CTransactionTemplateTypeEnum transactionTemplateTypeEnum)
    {
        var result = await Query()
            .Where(x => x.TransactionTemplateTypeId == (int)transactionTemplateTypeEnum)
            .Select(x => x.Data)
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Could not find a transaction template for type: {transactionTemplateTypeEnum}", result);

        return result;
    }

    private IQueryable<CTransactionTemplate> Query(bool asNoTracking = true)
    {
        var query = _db.CTransactionTemplates;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}