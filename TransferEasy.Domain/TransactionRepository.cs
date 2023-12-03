namespace TransferEasy.Domain;

public interface IStoreTransactions
{
    bool SaveTransaction(Transaction transaction);
    Transaction GetTransaction(Guid id);
    IEnumerable<Transaction> GetTransactions();
    IEnumerable<Transaction> GetTransactionsByAccount(int accountId);
}

public class TransactionRepository : IStoreTransactions
{
    public Transaction GetTransaction(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> GetTransactions()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> GetTransactionsByAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public bool SaveTransaction(Transaction transaction)
    {
        throw new NotImplementedException();
    }
}
