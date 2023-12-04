using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransferEasy.Domain;

/// <remarks>
/// Add migrations using the following command inside the project directory:
///
/// dotnet ef migrations add --context AccountContext [migration-name]
/// </remarks>
public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        builder.HasPostgresEnum<AccountNormality>();
        builder.HasPostgresEnum<AccountType>();

        builder.UseIdentityByDefaultColumns();

        // Seed
        builder.Entity<Account>().HasData(new Account
        {
            Id = Domain.AccountsBasic.CashAccountId,
            Name = "Cash",
            Normality = AccountNormality.DebitNormal,
            Type = AccountType.System
        });
        builder.Entity<Account>().HasData(new Account
        {
            Id = Domain.AccountsBasic.RevenueAccountId,
            Name = "Revenue from Fees",
            Normality = AccountNormality.CreditNormal,
            Type = AccountType.System
        });
        builder.Entity<Account>().HasData(new Account
        {
            Id = Domain.AccountsBasic.ExpensesAccountId,
            Name = "Card Processing Expenses",
            Normality = AccountNormality.DebitNormal,
            Type = AccountType.System
        });
    }
}

class AccountEntityTypeConfiguration
    : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.Property(ci => ci.Name)
            .HasMaxLength(50);

        builder.Property(a => a.Normality);
        builder.Property(a => a.Type);

        builder.Property(a => a.Id).HasIdentityOptions(startValue: 100);

        //builder.Ignore(ci => ci.PictureUri);
    }
}
