namespace TransferEasy.Domain;
public interface ILedgerService
{
    Transaction Transfer(decimal amount, int fromAccountId, int toAccountId);
    Transaction Deposit(decimal amount, int toAccountId);
    Transaction Withdraw(decimal amount, int fromAccountId);
}

public class LedgerService(IStoreTransactions transactionsRepository, IProvideBalance balanceProvider) : ILedgerService
{
    private const decimal CardProcessingFeePercentage = 2.0m;
    private const decimal WithdrawalFeePercentage = 3.0m;

    public Transaction Transfer(decimal amount, int fromAccountId, int toAccountId)
    {
        // Validate business
        var originAccountBalance = balanceProvider.GetBalance(fromAccountId);
        if (originAccountBalance < amount) 
            throw new InsufficientFundsException(originAccountBalance, amount);

        // Create transaction
        var originEntry = new Entry(amount, EntryDirection.Debit);
        var destinationEntry = new Entry(amount, EntryDirection.Credit);

        var entries = new List<TransactionEntry>()
        {
            new(fromAccountId, originEntry),
            new(toAccountId, destinationEntry)
        };

        // Add transaction
        var transaction = new Transaction(Guid.NewGuid(), entries, TransactionType.UserTransfer, DateTime.UtcNow);
        transactionsRepository.SaveTransaction(transaction);

        return transaction;
    }

    public Transaction Deposit(decimal amount, int toAccountId)
    {
        // Create transaction
        var cardProcessingFee = amount * CardProcessingFeePercentage / 100;

        var entries = new List<TransactionEntry>()
        {
            new(toAccountId, new (amount, EntryDirection.Credit)),
            new(AccountsBasic.CashAccountId, new(amount - cardProcessingFee, EntryDirection.Debit)),
            new(AccountsBasic.ExpensesAccountId, new(cardProcessingFee, EntryDirection.Debit))
        };

        // Add transaction
        var transaction = new Transaction(Guid.NewGuid(), entries, TransactionType.Deposit, DateTime.UtcNow);
        transactionsRepository.SaveTransaction(transaction);

        return transaction;
    }

    public Transaction Withdraw(decimal amount, int fromAccountId)
    {
        // Validate business
        var originAccountBalance = balanceProvider.GetBalance(fromAccountId);
        if (originAccountBalance < amount)
            throw new InsufficientFundsException(originAccountBalance, amount);

        // Create transaction
        var withdrawalFee = amount * WithdrawalFeePercentage / 100;

        var entries = new List<TransactionEntry>()
        {
            new(fromAccountId, new (amount, EntryDirection.Debit)),
            new(AccountsBasic.CashAccountId, new(amount - withdrawalFee, EntryDirection.Credit)),
            new(AccountsBasic.RevenueAccountId, new(withdrawalFee, EntryDirection.Credit))
        };

        // Add transaction
        var transaction = new Transaction(Guid.NewGuid(), entries, TransactionType.Withdrawal, DateTime.UtcNow);
        transactionsRepository.SaveTransaction(transaction);

        return transaction;
    }
}
