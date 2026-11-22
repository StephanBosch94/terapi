using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CCurrencyMapper
{
    public static readonly Expression<Func<CCurrency, CCurrencyVM>> AsVM =
        dataItem => new CCurrencyVM
        {
            CurrencyId = dataItem.CurrencyId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            Rate = dataItem.Rate,
            Symbol = dataItem.Symbol,
            IsBase = dataItem.IsBase
        };

    public static CCurrencyVM ToVm(CCurrency dataItem)
    {
        return new CCurrencyVM
        {
            CurrencyId = dataItem.CurrencyId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            Rate = dataItem.Rate,
            Symbol = dataItem.Symbol,
            IsBase = dataItem.IsBase
        };
    }

    public static IQueryable<CCurrencyVM> ToApiList(this IQueryable<CCurrency> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}