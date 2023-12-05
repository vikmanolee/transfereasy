namespace TransferEasy.Domain;

public record Transaction(Guid Id,
    IEnumerable<TransactionEntry> Entries,
    TransactionType Type,
    DateTime CreatedAt)
{
    public bool IsValid()
    {
        var debitSum = Entries.Where(entry => entry.Entry.Direction == EntryDirection.Debit).Sum(e => e.Entry.Amount);
        var creditSum = Entries.Where(entry => entry.Entry.Direction == EntryDirection.Credit).Sum(e => e.Entry.Amount);

        return debitSum == creditSum;
    }
}

public enum TransactionType
{
    UserTransfer,
    Deposit,
    Withdrawal
}
