using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CPaymentServiceProviderSeeder
{
    public static readonly List<CPaymentServiceProvider> Entities = CodetableUtil
        .GetEnumValues<CPaymentServiceProviderEnum>(CCodetablePrefixesEnum.CPaymentServiceProvider)
        .Select(
            x => new CPaymentServiceProvider
            {
                PaymentServiceProviderId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}