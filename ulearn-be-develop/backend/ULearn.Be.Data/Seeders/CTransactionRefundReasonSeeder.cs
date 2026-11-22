using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CTransactionRefundReasonSeeder
{
    public static readonly List<CTransactionRefundReason> Entities = CodetableUtil
        .GetEnumValues<CTransactionRefundReasonEnum>(CCodetablePrefixesEnum.CTransactionRefundReason)
        .Select(
            x => new CTransactionRefundReason
            {
                TransactionRefundReasonId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}