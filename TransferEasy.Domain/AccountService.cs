namespace TransferEasy.Domain;

public interface IAccountService
{
    int AddCustomerAccount(string username);
    int AddSystemAccount(string name, AccountNormality normality);
    Account GetAccount(int accountId);
    IEnumerable<Account> GetAllAccounts();
    IEnumerable<Account> GetSystemAccounts();
    IEnumerable<Account> GetCustomerAccounts();

    // Updates and deletions not available yet in our MVP :)
}

public class AccountService(IStoreAccounts accountsRepository) : IAccountService
{
    public int AddCustomerAccount(string username)
    {
        return accountsRepository.AddAccount(new Account() { 
            Name = username,
            Type = AccountType.Customer,
            Normality = AccountNormality.CreditNormal
        });
    }

    public int AddSystemAccount(string name, AccountNormality normality)
    {
        return accountsRepository.AddAccount(new Account()
        {
            Name = name,
            Type = AccountType.System,
            Normality = normality
        });
    }

    public Account GetAccount(int accountId)
    {
        return accountsRepository.GetAccount(accountId);
    }

    public IEnumerable<Account> GetAllAccounts()
    {
        return accountsRepository.GetAccounts();
    }

    public IEnumerable<Account> GetCustomerAccounts()
    {
        return accountsRepository.GetAccounts()
            .Where(account =>  account.Type == AccountType.Customer);
    }

    public IEnumerable<Account> GetSystemAccounts()
    {
        return accountsRepository.GetAccounts()
            .Where(account => account.Type == AccountType.System);
    }
}
