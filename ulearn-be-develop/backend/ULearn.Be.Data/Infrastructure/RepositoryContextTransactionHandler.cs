using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Infrastructure.Interfaces;

namespace ULearn.Be.Data.Infrastructure;

public class RepositoryContextTransactionHandler : IDbContextTransactionHandler
{
    private readonly RepositoryContext _db;

    public RepositoryContextTransactionHandler(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<T> WithTransaction<T>(Func<Task<T>> handler) where T : class
    {
        var strategy = _db.Database.CreateExecutionStrategy();

        T result = null;
        await strategy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();
                result = await handler();
                await transaction.CommitAsync();
            }
        );

        return result;
    }

    public async Task WithTransaction(Func<Task> handler)
    {
        var strategy = _db.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();
                await handler();
                await transaction.CommitAsync();
            }
        );
    }
}