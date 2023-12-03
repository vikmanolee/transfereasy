namespace TransferEasy.Domain;

public interface IApplicationService
{
    void Transfer(decimal amount, int fromAccountId, int toAccountId);
    void Deposit(decimal amount, int toAccountId);
    void Withdraw(decimal amount, int fromAccountId);
    IEnumerable<Account> GetCustomerAccounts();
    IEnumerable<Account> GetSystemAccounts();
    Account GetAccount(int accountId);
    int CreateAccount(string username);
    decimal GetAccountBalance(int accountId);
}

public class ApplicationService(IProvideBalance balanceProvider, IPublisher commandPublisher, IAccountService accountService) : IApplicationService
{
    public void Deposit(decimal amount, int toAccountId)
    {
        commandPublisher.Publish(new DepositCommand(toAccountId, amount));
    }

    public void Transfer(decimal amount, int fromAccountId, int toAccountId)
    {
        commandPublisher.Publish(new TransferCommand(fromAccountId, toAccountId, amount));
    }

    public void Withdraw(decimal amount, int fromAccountId)
    {
        commandPublisher.Publish(new WithdrawCommand(fromAccountId, amount));
    }

    public int CreateAccount(string username)
    {
        return accountService.AddCustomerAccount(username);
    }

    public IEnumerable<Account> GetCustomerAccounts() => accountService.GetCustomerAccounts();
    public IEnumerable<Account> GetSystemAccounts() => accountService.GetSystemAccounts();

    public Account GetAccount(int accountId)
    {
        return accountService.GetAccount(accountId);
    }

    public decimal GetAccountBalance(int accountId)
    {
        return balanceProvider.GetBalance(accountId);
    }
}
