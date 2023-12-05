namespace TransferEasy.Domain;

public class InsufficientFundsException(decimal current, decimal requested) 
    : Exception($"Insufficient funds to complete transaction. Cannot remove {requested:F3}, from {current:F3}")
{
}
