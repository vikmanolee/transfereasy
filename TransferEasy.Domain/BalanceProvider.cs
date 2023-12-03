namespace TransferEasy.Domain;

public interface IProvideBalance
{
    double GetBalance(int accountId);
    void UpdateBalances(IEnumerable<TransactionEntry> transactionEntries);
}

public class BalanceProvider(ICacheAccountBalance accountsCache, IAccountService accountService, IStoreTransactions transactionsRepository) : IProvideBalance
{
    public double GetBalance(int accountId)
    {
        var balance = accountsCache.GetAccount(accountId)?.CurrentBalance;

        if (balance != null)
        {
            return balance.Value;
        }

        var account = accountService.GetAccount(accountId);
        var transactions = transactionsRepository.GetTransactionsByAccount(accountId);
        var newBalance = GetBalanceInternal(account, transactions);
        accountsCache.AddAccount(new AccountBalance() { CurrentBalance = newBalance, Id = accountId });
        return newBalance;
    }

    public void UpdateBalances(IEnumerable<TransactionEntry> transactionEntries)
    {
        foreach (var entry in transactionEntries)
        {
            accountsCache.RemoveAccount(entry.AccountId);
        }
    }

    private static double GetBalanceInternal(Account account, IEnumerable<Transaction> transactions)
    {
        var entries = transactions.SelectMany(tr => tr.Entries).Select(e => e.Entry);

        switch (account.Normality)
        {
            case AccountNormality.CreditNormal:
                return entries.Aggregate(0d,
                    (current, entry) => entry.Direction == EntryDirection.Credit
                    ? current + entry.Amount
                    : current - entry.Amount);
            case AccountNormality.DebitNormal:
                return entries.Aggregate(0d,
                    (current, entry) => entry.Direction == EntryDirection.Credit
                    ? current - entry.Amount
                    : current + entry.Amount);
            default:
                return 0;
        }
    }
}
