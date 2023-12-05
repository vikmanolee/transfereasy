namespace TransferEasy.Domain;

public record TransactionEntry(int AccountId, Entry Entry);

public record Entry(decimal Amount, EntryDirection Direction);

public enum EntryDirection
{
    Debit,
    Credit
}
