namespace TransferEasy.Web;

public class TransferEasyApiClient(HttpClient httpClient)
{

    public async Task InitAsync()
    {
        await httpClient.GetAsync("/init");
    }

    public async Task<Account[]> GetAccountsAsync()
    {
        return await httpClient.GetFromJsonAsync<Account[]>("/accounts") ?? [];
    }

    public async Task<Account[]> GetSystemAccountsAsync()
    {
        return await httpClient.GetFromJsonAsync<Account[]>("/accounts/system") ?? [];
    }

    public async Task CreateAccountAsync(string username)
    {
        await httpClient.PostAsJsonAsync("/accounts", new CreateAccountRequest(username));
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        return decimal.TryParse(await httpClient.GetStringAsync($"/accounts/{accountId}"), out decimal result) ? result : 0m;
    }
}

public record Account(int Id, string Name, AccountNormality Normality);

public record CreateAccountRequest(string Username);

public enum AccountNormality
{
    CreditNormal = 1,
    DebitNormal = 2
}
