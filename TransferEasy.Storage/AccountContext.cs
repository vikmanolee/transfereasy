using Microsoft.EntityFrameworkCore;

namespace TransferEasy.Domain;

/// <remarks>
/// Add migrations using the following command inside the 'Catalog.API' project directory:
///
/// dotnet ef migrations add --context AccountContext [migration-name]
/// </remarks>
public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {
    }

    public DbSet<CatalogItem> AccountItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());

        // Add the outbox table to this context
        builder.UseIntegrationEventLogs();
    }
}