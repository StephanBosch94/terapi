namespace ULearn.Be.Models.InputModels;

public class AppTransactionGroupIM
{
    public AppTransactionGroupIM(params AppTransactionIM[] transactions)
    {
        Transactions = transactions;
    }

    public ICollection<AppTransactionIM> Transactions { get; set; }
}