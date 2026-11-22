using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CInterestMapper
{
    public static readonly Expression<Func<CInterest, CInterestVM>> AsVM =
        dataItem => new CInterestVM
        {
            InterestId = dataItem.InterestId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            IconName = dataItem.IconName
        };

    public static CInterestVM ToVm(CInterest dataItem)
    {
        return new CInterestVM
        {
            InterestId = dataItem.InterestId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            IconName = dataItem.IconName
        };
    }

    public static IQueryable<CInterestVM> ToApiList(this IQueryable<CInterest> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}