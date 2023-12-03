namespace TransferEasy.Domain;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(double current, double requested) 
        : base($"Insufficient funds to complete transaction. Cannot transfer {requested}, from {current}")
    {
    }
}
