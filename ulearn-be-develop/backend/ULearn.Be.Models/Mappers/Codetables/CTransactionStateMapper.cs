using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CTransactionStateMapper
{
    public static CTransactionStateVM ToVm(CTransactionState dataItem)
    {
        return new CTransactionStateVM
        {
            TransactionStateId = dataItem.TransactionStateId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
        };
    }
}