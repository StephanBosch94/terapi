using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CCategoryMapper
{
    public static readonly Expression<Func<CCategory, CCategoryVM>> AsVM =
        dataItem => new CCategoryVM
        {
            CategoryId = dataItem.CategoryId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static IQueryable<CCategoryVM> ToApiList(this IQueryable<CCategory> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}