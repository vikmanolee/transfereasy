using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TransferEasy.Domain;

public interface ICacheAccountBalance
{
    AccountBalance? GetAccount(int accountId);
    void AddAccount(AccountBalance account);
    void RemoveAccount(int accountId);
}

public class AccountBalanceCache(IDistributedCache cache) : ICacheAccountBalance
{
    public void AddAccount(AccountBalance account)
    {
        cache.SetString(CacheKey(account.Id), JsonSerializer.Serialize(account));
    }

    public AccountBalance? GetAccount(int accountId)
    {
        var cached = cache.GetString(CacheKey(accountId));

        return cached != null ? JsonSerializer.Deserialize<AccountBalance>(cached) : null;
    }

    public void RemoveAccount(int accountId)
    {
        cache.Remove(CacheKey(accountId));
    }

    private static string CacheKey(int accountId) => $"account_balance_{accountId}";
}
