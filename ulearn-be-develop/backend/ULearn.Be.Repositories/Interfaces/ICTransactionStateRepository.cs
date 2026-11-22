using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICTransactionStateRepository
{
    Task<CTransactionState> GetRawDbEntity(int transactionStateId);
}