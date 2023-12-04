using TransferEasy.Domain;

namespace TransferEasy.ApiService;

public static class TransferEasyEndpoints
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
            applicationService.Transfer(request.Amount, accountId, request.ToAccountId);
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

        app.MapGet("/init", (AccountContext context) =>
        {
            context.Database.EnsureCreated();
        });
    }
}

record TransferRequest(int ToAccountId, decimal Amount);
record DepositRequest(decimal Amount);
record WithdrawalRequest(decimal Amount);

record CreateAccountRequest(string Username);
