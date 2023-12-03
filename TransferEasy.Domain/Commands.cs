namespace TransferEasy.Domain
{
    public record TransferCommand(int OriginAccount, int DestinationAccount, double Amount);
    public record DepositCommand(int DestinationAccount, double Amount);
    public record WithdrawCommand(int OriginAccount, double Amount);
}
