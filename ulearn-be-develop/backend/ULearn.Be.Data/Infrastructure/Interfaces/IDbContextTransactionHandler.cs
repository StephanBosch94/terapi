namespace ULearn.Be.Data.Infrastructure.Interfaces;

public interface IDbContextTransactionHandler
{
    Task<T> WithTransaction<T>(Func<Task<T>> handler) where T : class;
    Task WithTransaction(Func<Task> handler);
}