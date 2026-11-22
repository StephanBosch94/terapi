using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.ViewModels;
using ULearn.Be.Data.Context;

namespace ULearn.Be.Repositories.Base;

public abstract class BaseRepository
{
    private readonly RepositoryContext _db;

    protected BaseRepository(RepositoryContext db)
    {
        _db = db;
    }

    protected static void ThrowIfEmpty<T>(int id, ICollection<T> entity, string idName)
    {
        ThrowIfEmpty(
            $"Could not find any records of {typeof(T).Name} for {idName ?? "Id"} :  {id}",
            entity
        );
    }

    protected static void ThrowIfEmpty<T>(ICollection<int> ids, ICollection<T> entity, string idName)
    {
        ThrowIfEmpty(
            $"Could not find any records of {typeof(T).Name} for {idName ?? "Ids"} :  {string.Join(',', ids)}",
            entity
        );
    }

    protected static void ThrowIfNotFound<T>(int id, T entity)
    {
        ThrowIfNotFound($"Could not find {typeof(T).Name} with ID: {id}", entity);
    }

    protected static void ThrowIfNotFound(string message, object entity)
    {
        if (entity == null)
        {
            throw new NotFoundException(message);
        }
    }

    protected static void ThrowIfEmpty<T>(string message, ICollection<T> collection)
    {
        if (collection == null || collection.Count == 0)
        {
            throw new NotFoundException(message);
        }
    }

    protected static void ThrowIfNotFound(string message, bool exists)
    {
        if (!exists)
        {
            throw new NotFoundException(message);
        }
    }

    protected static void ThrowIfNotExists(bool exists, string message = "")
    {
        if (!exists)
        {
            throw new NotFoundException(message);
        }
    }

    protected Task<int> ApplyChanges<TEntity>(CompareResultVM<TEntity> compareResultVm, Action<TEntity, TEntity> updateAction) where TEntity: class
    {
        foreach (var (dbEntity, newerEntity) in compareResultVm.ToUpdate)
        {
            updateAction(dbEntity, newerEntity);
        }
        
        _db.Set<TEntity>().RemoveRange(compareResultVm.ToRemove);
        _db.Set<TEntity>().AddRange(compareResultVm.ToAdd);    
        
        return _db.SaveChangesAsync();
    }
}