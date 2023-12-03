namespace TransferEasy.Domain;

public interface IStoreAccounts
{
    int AddAccount(Account account);
    Account GetAccount(int accountId);
    IEnumerable<Account> GetAccounts();
}

public class AccountsRepository : IStoreAccounts
{
    public int AddAccount(Account account)
    {
        throw new NotImplementedException();
    }

    public Account GetAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Account> GetAccounts()
    {
        throw new NotImplementedException();
    }
}
