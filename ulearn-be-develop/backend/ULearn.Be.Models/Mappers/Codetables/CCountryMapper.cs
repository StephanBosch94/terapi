using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CCountryMapper
{
    public static readonly Expression<Func<CCountry, CCountryVM>> AsVM =
        dataItem => new CCountryVM
        {
            CountryId = dataItem.CountryId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            CountryCode = dataItem.CountryCode
        };

    public static CCountryVM ToVm(CCountry dataItem)
    {
        return new CCountryVM
        {
            CountryId = dataItem.CountryId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            CountryCode = dataItem.CountryCode
        };
    }

    public static IQueryable<CCountryVM> ToApiList(this IQueryable<CCountry> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}