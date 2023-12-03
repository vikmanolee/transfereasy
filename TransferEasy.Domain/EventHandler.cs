namespace TransferEasy.Domain;

public interface IHandleEvent
{
    void HandleEvent(TransactionEvent transactionEvent);
}

public class EventHandler(IProvideBalance balanceProvider) : IHandleEvent
{
    public void HandleEvent(TransactionEvent transactionEvent)
    {
        balanceProvider.UpdateBalances(transactionEvent.Entries);
    }
}
