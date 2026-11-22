using System.Text.RegularExpressions;
using ULearn.Be.Common.ViewModels;

namespace ULearn.Be.Common.Utils;

public static class CompareUtil
{
    public static bool JsonEquals(string jsonStr1, string jsonStr2)
    {
        if (jsonStr1 == null && jsonStr2 == null)
        {
            return true;
        }

        if (jsonStr1 == null || jsonStr2 == null)
        {
            return false;
        }

        var jsonStr1Replaced = Regex.Replace(jsonStr1, @"\s+", "");
        var jsonStr2Replaced = Regex.Replace(jsonStr2, @"\s+", "");
        return string.Equals(jsonStr1Replaced, jsonStr2Replaced, StringComparison.CurrentCultureIgnoreCase);
    }

    public static CompareResultVM<T1> Compare<T1, T2>(
        ICollection<T1> newCollection,
        ICollection<T1> oldCollection,
        Func<T1, T2> getKey
    )
    {
        newCollection ??= new List<T1>();
        oldCollection ??= new List<T1>();
        return new CompareResultVM<T1>
        {
            ToAdd = newCollection.Where(
                x => oldCollection.All(y => !EqualityComparer<T2>.Default.Equals(getKey(x), getKey(y)))
            ).ToList(),
            ToRemove = oldCollection.Where(
                x => newCollection.All(y => !EqualityComparer<T2>.Default.Equals(getKey(x), getKey(y)))
            ).ToList(),
            ToUpdate = oldCollection.Where(
                x => newCollection.Any(y => EqualityComparer<T2>.Default.Equals(getKey(x), getKey(y)))
            ).Select(
                dbEntity => (dbEntity, newCollection.First(
                    pkgEntity =>
                        EqualityComparer<T2>.Default.Equals(getKey(dbEntity), getKey(pkgEntity))
                ))
            ).ToList()
        };
    }

    public static (IEnumerable<T1> toDelete, IEnumerable<T1> toAssign) GetUpdatedCollection<T1, T2>(
        ICollection<T1> dbCollection,
        ICollection<T1> newerCollection,
        Func<T1, T2> getKey,
        Action<T1, T1> updateAction
    )
    {
        var compareResult = Compare(newerCollection, dbCollection, getKey);
        var updated = compareResult.ToUpdate.Select(
            (entities, i) =>
            {
                updateAction(entities.dbEntity, entities.newerEntity);
                return entities.dbEntity;
            }
        );
        return (
            compareResult.ToRemove,
            compareResult.ToAdd.Concat(updated).Except(compareResult.ToRemove)
        );
    }

    public static IEnumerable<T> ToUpdated<T>(this CompareResultVM<T> compareResult)
    {
        return compareResult.ToAdd
            .Concat(compareResult.ToUpdate.Select(x => x.dbEntity))
            .Except(compareResult.ToRemove);
    }
}