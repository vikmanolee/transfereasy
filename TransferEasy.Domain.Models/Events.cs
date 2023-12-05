namespace TransferEasy.Domain;

public record TransactionEvent(Guid Id,
    IEnumerable<TransactionEntry> Entries,
    TransactionType Type,
    DateTime CreatedAt);

public record UserTransferEvent : TransactionEvent
{
    public UserTransferEvent(Transaction transaction)
        : base(transaction.Id, transaction.Entries, transaction.Type, transaction.CreatedAt)
    {
    }
}

public record DepositEvent : TransactionEvent
{
    public DepositEvent(Transaction transaction)
    : base(transaction.Id, transaction.Entries, transaction.Type, transaction.CreatedAt)
    {
    }
}

public record WithdrawalEvent : TransactionEvent
{
    public WithdrawalEvent(Transaction transaction)
        : base(transaction.Id, transaction.Entries, transaction.Type, transaction.CreatedAt)
    {
    }
}
