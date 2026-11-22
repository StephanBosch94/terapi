using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CMasterCacheMapper
{
    public static readonly Expression<Func<CMasterHash, CMasterCacheVM>> AsVM =
        dataItem => new CMasterCacheVM
        {
            MasterCacheId = dataItem.MasterHashId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            TableName = dataItem.TableName,
            Hash = dataItem.Hash.ToString()
        };

    public static IQueryable<CMasterCacheVM> ToApiList(this IQueryable<CMasterHash> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}