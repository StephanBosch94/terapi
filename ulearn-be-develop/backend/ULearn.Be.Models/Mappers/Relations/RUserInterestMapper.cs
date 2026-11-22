using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RUserInterestMapper
{
    public static readonly Expression<Func<RUserInterest, CInterestVM>> AsVM =
        dataItem => new CInterestVM
        {
            UserId = dataItem.UserId,
            EnumKey = dataItem.CInterest.EnumKey,
            NameKey = dataItem.CInterest.NameKey,
            DescriptionKey = dataItem.CInterest.DescriptionKey,
            IconName = dataItem.CInterest.IconName,
        };

    public static IQueryable<CInterestVM> ToApiList(this IQueryable<RUserInterest> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}