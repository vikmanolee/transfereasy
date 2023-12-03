namespace TransferEasy.Web;

public class TransferEasyApiClient(HttpClient httpClient)
{
    public async Task<Account[]> GetAccountsAsync()
    {
        return await httpClient.GetFromJsonAsync<Account[]>("/accounts") ?? [];
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        return decimal.TryParse(await httpClient.GetStringAsync($"/accounts/{accountId}"), out decimal result) ? result : 0m;
    }
}

public record Account(int Id, string Name);
