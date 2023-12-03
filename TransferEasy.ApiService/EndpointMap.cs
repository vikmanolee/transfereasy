using TransferEasy.Domain;

namespace TransferEasy.ApiService;

public static class AppEndpoints
{
    public static void Map(WebApplication app)
    {
        MapTransactionEndpoints(app);
        MapAccountEndpoints(app);
    }

    private static void MapTransactionEndpoints(WebApplication app)
    {
        app.MapPost("/transfer/{accountId}", (int accountId, TransferRequest request, IApplicationService applicationService) =>
        {
            applicationService.Transfer(request.Amount, accountId, request.toAccountId);
        });

        app.MapPost("/deposit/{accountId}", (int accountId, DepositRequest request, IApplicationService applicationService) =>
        {
            applicationService.Deposit(request.Amount, accountId);
        });

        app.MapPost("/withdraw/{accountId}", (int accountId, WithdrawalRequest request, IApplicationService applicationService) =>
        {
            applicationService.Withdraw(request.Amount, accountId);
        });
    }

    private static void MapAccountEndpoints(WebApplication app)
    {
        app.MapPost("/accounts", (CreateAccountRequest request, IApplicationService applicationService) =>
        {
            return applicationService.CreateAccount(request.Username);
        });

        app.MapGet("/accounts/{accountId}/balance", (int accountId, IApplicationService applicationService) =>
        {
            return applicationService.GetAccountBalance(accountId);
        });

        app.MapGet("/accounts/{accountId}", (int accountId, IApplicationService applicationService) =>
        {
            return applicationService.GetAccountBalance(accountId);
        });

        app.MapGet("/accounts", (IApplicationService applicationService) =>
        {
            return applicationService.GetCustomerAccounts();
        });

        app.MapGet("/accounts/system", (IApplicationService applicationService) =>
        {
            return applicationService.GetSystemAccounts();
        });
    }
}

record TransferRequest(int toAccountId, double Amount);
record DepositRequest(double Amount);
record WithdrawalRequest(double Amount);

record CreateAccountRequest(string Username);
