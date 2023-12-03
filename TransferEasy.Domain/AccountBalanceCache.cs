namespace TransferEasy.Domain;

public interface ICacheAccountBalance
{
    AccountBalance? GetAccount(int accountId);
    void AddAccount(AccountBalance account);
    void UpdateAccount(AccountBalance account);
    void RemoveAccount(int accountId);
}

public class AccountBalanceCache : ICacheAccountBalance
{
    public void AddAccount(AccountBalance account)
    {
        throw new NotImplementedException();
    }

    public AccountBalance? GetAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public void RemoveAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public void UpdateAccount(AccountBalance account)
    {
        throw new NotImplementedException();
    }
}
