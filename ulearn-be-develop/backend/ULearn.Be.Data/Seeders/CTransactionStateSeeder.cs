using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CTransactionStateSeeder
{
    public static readonly List<CTransactionState> Entities = CodetableUtil
        .GetEnumValues<CTransactionStateEnum>(CCodetablePrefixesEnum.CTransactionState)
        .Select(
            x => new CTransactionState
            {
                TransactionStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}