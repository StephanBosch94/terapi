using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CPaymentServiceProviderMapper
{
    public static readonly Expression<Func<CPaymentServiceProvider, CPaymentServiceProviderVM>> AsVM =
        dataItem => new CPaymentServiceProviderVM
        {
            PaymentServiceProviderId = dataItem.PaymentServiceProviderId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static IQueryable<CPaymentServiceProviderVM> ToApiList(this IQueryable<CPaymentServiceProvider> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}