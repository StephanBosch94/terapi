using ULearn.Be.Data.Enums;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICTransactionTemplateRepository
{
    Task Upsert(Stream stream, string fileContentType, CTransactionTemplateTypeEnum transactionTypeEnum);
    Task<byte[]> GetTemplateData(CTransactionTemplateTypeEnum transactionTemplateTypeEnum);
}