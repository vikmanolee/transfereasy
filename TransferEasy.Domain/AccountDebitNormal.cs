namespace TransferEasy.Domain;

public class AccountDebitNormal : Account
{
    public AccountDebitNormal(int id, string name)
    {
        Name = name;
        Id = id;
    }

    public override double GetBalance() => Entries.Aggregate(0d,
    (current, entry) => entry.Direction == EntryDirection.Credit
    ? current - entry.Amount
    : current + entry.Amount);
}
