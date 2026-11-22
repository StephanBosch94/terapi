using ULearn.Be.Data.Enums;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICTransactionTemplateTypeRepository
{
    Task<CTransactionTemplateTypeEnum> GetTransactionTemplateTypeEnum(string transactionTemplateTypeKey);
}