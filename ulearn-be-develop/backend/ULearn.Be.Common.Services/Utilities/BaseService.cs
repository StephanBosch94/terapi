using JetBrains.Annotations;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;

namespace ULearn.Be.Common.Services.Utilities;

public abstract class BaseService
{
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

    [ContractAnnotation("entity:null => halt")]
    protected static void ThrowIfNotFound<T>(int id, T entity)
    {
        ThrowIfNotFound($"Could not find {typeof(T).Name} with ID: {id}", entity);
    }

    [ContractAnnotation("entity:null => halt")]
    protected static void ThrowIfNotFoundUsingKey<T>(string key, T entity)
    {
        ThrowIfNotFound($"Could not find {typeof(T).Name} with Key: {key}", entity);
    }

    [ContractAnnotation("entity:null => halt")]
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

    [ContractAnnotation("exists:false => halt")]
    protected static void ThrowIfNotExists(bool exists, string message = "")
    {
        if (!exists)
        {
            throw new NotFoundException(message);
        }
    }

    [ContractAnnotation("exists:true => halt")]
    protected static void ThrowIfExists(bool exists)
    {
        if (exists)
        {
            throw new ConflictException(ErrorCodesEnum.Exists);
        }
    }
}