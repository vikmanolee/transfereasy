using TransferEasy.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ILedgerService, LedgerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/addAccount/{username}", (string username, ILedgerService ledgerService) =>
{
    return ledgerService.AddAccount(username);
});

app.MapGet("/getBalance/{username}", (string username, ILedgerService ledgerService) =>
{
    return ledgerService.GetAccountBalance(username);
});

app.MapPost("/deposit/{accountId}", (int accountId, DepositRequest request, ILedgerService ledgerService) =>
{
     ledgerService.Deposit(request.Amount, accountId);
});

app.MapPost("/withdraw/{accountId}", (int accountId, WithdrawalRequest request, ILedgerService ledgerService) =>
{
    ledgerService.Withdraw(request.Amount, accountId);
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record DepositRequest(double Amount);

record WithdrawalRequest(double Amount);
