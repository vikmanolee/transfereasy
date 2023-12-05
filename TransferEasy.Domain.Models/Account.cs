namespace TransferEasy.Domain;

public class Account
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public AccountNormality Normality { get; set; }
    public AccountType Type { get; set; }
}

public enum AccountNormality
{
    CreditNormal = 1,
    DebitNormal = 2
}

public enum AccountType
{
    System = 0,
    Customer
}
