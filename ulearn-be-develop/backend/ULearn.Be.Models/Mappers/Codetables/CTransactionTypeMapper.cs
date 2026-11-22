using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public class CTransactionTypeMapper
{
    public static CTransactionTypeVM ToVm(CTransactionType dataItem)
    {
        return new CTransactionTypeVM
        {
            TransactionTypeId = dataItem.TransactionTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
        };
    }
}