namespace TransferEasy.Domain;

public record TransactionEntry(int AccountId, Entry Entry);

public record Entry(double Amount, EntryDirection Direction);

public enum EntryDirection
{
    Debit,
    Credit
}
