using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICTransactionRefundReasonRepository
{
    Task<CTransactionRefundReason> GetRawDbEntity(int transactionRefundReasonId);
}