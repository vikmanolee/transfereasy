namespace TransferEasy.Domain
{
    public record TransferCommand(int OriginAccount, int DestinationAccount, decimal Amount);
    public record DepositCommand(int DestinationAccount, decimal Amount);
    public record WithdrawCommand(int OriginAccount, decimal Amount);
}
