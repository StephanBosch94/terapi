using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CTransactionTemplateTypeSeeder
{
    public static readonly List<CTransactionTemplateType> Entities = CodetableUtil
        .GetEnumValues<CTransactionTemplateTypeEnum>(CCodetablePrefixesEnum.CTransactionTemplateType)
        .Select(
            x => new CTransactionTemplateType
            {
                TransactionTemplateTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}