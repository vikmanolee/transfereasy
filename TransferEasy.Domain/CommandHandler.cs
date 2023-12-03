namespace TransferEasy.Domain;

public interface IHandleCommand
{
    void Handle(TransferCommand transfer);
    void Handle(DepositCommand deposit);
    void Handle(WithdrawCommand withdraw);
}

public class CommandHandler(ILedgerService ledgerService, IPublisher publisher) : IHandleCommand
{
    public void Handle(TransferCommand transfer)
    {
        var transaction = ledgerService.Transfer(transfer.Amount, transfer.OriginAccount, transfer.DestinationAccount);

        publisher.Publish(new UserTransferEvent(transaction));
    }

    public void Handle(DepositCommand deposit)
    {
        var transaction = ledgerService.Deposit(deposit.Amount, deposit.DestinationAccount);

        publisher.Publish(new DepositEvent(transaction));
    }

    public void Handle(WithdrawCommand withdraw)
    {
        var transaction = ledgerService.Withdraw(withdraw.Amount, withdraw.OriginAccount);

        publisher.Publish(new WithdrawalEvent(transaction));
    }
}
