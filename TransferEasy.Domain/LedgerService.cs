namespace TransferEasy.Domain;
public interface ILedgerService
{
    Transaction Transfer(double amount, int fromAccountId, int toAccountId);
    Transaction Deposit(double amount, int toAccountId);
    Transaction Withdraw(double amount, int fromAccountId);
}

public class LedgerService(IStoreTransactions transactionsRepository, IProvideBalance balanceProvider) : ILedgerService
{
    private const int CashAccountId = 1;
    private const int RevenueAccountId = 2;
    private const int ExpensesAccountId = 3;

    private const double CardProcessingFeePercentage = 2.0;
    private const double WithdrawalFeePercentage = 3.0;

    public Transaction Transfer(double amount, int fromAccountId, int toAccountId)
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

    public Transaction Deposit(double amount, int toAccountId)
    {
        // Create transaction
        var cardProcessingFee = amount * CardProcessingFeePercentage / 100;

        var entries = new List<TransactionEntry>()
        {
            new(toAccountId, new (amount, EntryDirection.Credit)),
            new(CashAccountId, new(amount - cardProcessingFee, EntryDirection.Debit)),
            new(ExpensesAccountId, new(cardProcessingFee, EntryDirection.Debit))
        };

        // Add transaction
        var transaction = new Transaction(Guid.NewGuid(), entries, TransactionType.Deposit, DateTime.UtcNow);
        transactionsRepository.SaveTransaction(transaction);

        return transaction;
    }

    public Transaction Withdraw(double amount, int fromAccountId)
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
            new(CashAccountId, new(amount - withdrawalFee, EntryDirection.Credit)),
            new(RevenueAccountId, new(withdrawalFee, EntryDirection.Credit))
        };

        // Add transaction
        var transaction = new Transaction(Guid.NewGuid(), entries, TransactionType.Withdrawal, DateTime.UtcNow);
        transactionsRepository.SaveTransaction(transaction);

        return transaction;
    }
}
