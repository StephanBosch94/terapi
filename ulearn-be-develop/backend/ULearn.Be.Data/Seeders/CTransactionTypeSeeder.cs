using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CTransactionTypeSeeder
{
    public static readonly List<CTransactionType> Entities = CodetableUtil
        .GetEnumValues<CTransactionTypeEnum>(CCodetablePrefixesEnum.CTransactionType)
        .Select(
            x => new CTransactionType()
            {
                TransactionTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}