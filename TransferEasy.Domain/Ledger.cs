namespace TransferEasy.Domain;

public class Ledger(int id, string name, IEnumerable<Account> accounts)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public IEnumerable<Account> Accounts { get; set; } = accounts;
    public List<Transaction> Transactions { get; set; } = [];
}
