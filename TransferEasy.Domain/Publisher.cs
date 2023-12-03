namespace TransferEasy.Domain;

public interface IPublisher
{
    void Publish(TransactionEvent @event);
    void Publish(TransferCommand transfer);
    void Publish(DepositCommand deposit);
    void Publish(WithdrawCommand withdraw);
}

public class Publisher : IPublisher
{
    public void Publish(TransactionEvent @event)
    {
        throw new NotImplementedException();
    }

    public void Publish(TransferCommand transfer)
    {
        throw new NotImplementedException();
    }

    public void Publish(DepositCommand deposit)
    {
        throw new NotImplementedException();
    }

    public void Publish(WithdrawCommand withdraw)
    {
        throw new NotImplementedException();
    }
}


