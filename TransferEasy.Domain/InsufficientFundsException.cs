namespace TransferEasy.Domain;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(decimal current, decimal requested) 
        : base($"Insufficient funds to complete transaction. Cannot transfer {requested:F3}, from {current:F3}")
    {
    }
}
