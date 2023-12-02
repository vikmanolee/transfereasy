namespace TransferEasy.Domain;

public abstract class Account
{
    public string Name { get; set; }
    public int Id { get; set; }
    public List<Entry> Entries { get; set; } = [];

    public abstract double GetBalance();
}
