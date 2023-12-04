namespace TransferEasy.Domain;

public interface IStoreAccounts
{
    int AddAccount(Account account);
    Account GetAccount(int accountId);
    IEnumerable<Account> GetAccounts();
}

public class AccountsRepository(AccountContext dbContext) : IStoreAccounts
{
    public int AddAccount(Account account)
    {
        dbContext.Accounts.Add(account);
        dbContext.SaveChanges();
        return account.Id;
    }

    public Account GetAccount(int accountId) => dbContext.Accounts.First(account => account.Id == accountId);

    public IEnumerable<Account> GetAccounts() => dbContext.Accounts.ToList();
}
