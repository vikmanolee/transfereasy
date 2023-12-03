using TransferEasy.Domain;

namespace TransferEasy.ApiService;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ILedgerService, LedgerService>();
        services.AddScoped<IProvideBalance, BalanceProvider>();

        services.AddScoped<ICacheAccountBalance, AccountBalanceCache>();

        services.AddScoped<IStoreAccounts, AccountsRepository>();
        services.AddScoped<IStoreTransactions, TransactionRepository>();

        services.AddScoped<IHandleCommand, CommandHandler>();
        services.AddScoped<IHandleEvent, Domain.EventHandler>();
        services.AddScoped<IPublisher, Publisher>();

        return services;
    }
}
