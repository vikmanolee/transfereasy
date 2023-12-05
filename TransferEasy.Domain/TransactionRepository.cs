using Microsoft.Azure.Cosmos;
using System.ComponentModel;

namespace TransferEasy.Domain;

public interface IStoreTransactions
{
    Task SaveTransaction(Transaction transaction);
    Task<Transaction> GetTransaction(Guid id);
    Task<IEnumerable<Transaction>> GetTransactions();
    Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId);
}

public class TransactionRepository(CosmosClient cosmosClient) : IStoreTransactions
{
    public async Task<Transaction> GetTransaction(Guid id)
    {
        var container = GetContainer();
        ItemResponse<Transaction> response = await container.ReadItemAsync<Transaction>(
            id: id.ToString(),
            partitionKey: new PartitionKey("transactions")
        );

        return response;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var container = GetContainer();
        var query = new QueryDefinition("SELECT * FROM transactions");

        using FeedIterator<Transaction> feed = container.GetItemQueryIterator<Transaction>(query);

        List<Transaction> items = [];
        double requestCharge = 0d;
        while (feed.HasMoreResults)
        {
            FeedResponse<Transaction> response = await feed.ReadNextAsync();
            foreach (Transaction item in response)
            {
                items.Add(item);
            }
            requestCharge += response.RequestCharge;
        }
        return items;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId)
    {
        var container = GetContainer();
        var query = new QueryDefinition("SELECT * FROM transactions t WHERE t.accountId = @accountId")
            .WithParameter("@accountId", accountId);

        using FeedIterator<Transaction> feed = container.GetItemQueryIterator<Transaction>(query);

        List<Transaction> items = [];
        double requestCharge = 0d;
        while (feed.HasMoreResults)
        {
            FeedResponse<Transaction> response = await feed.ReadNextAsync();
            foreach (Transaction item in response)
            {
                items.Add(item);
            }
            requestCharge += response.RequestCharge;
        }
        return items;
    }

    public async Task SaveTransaction(Transaction transaction)
    {
        var container = GetContainer();
        await container.UpsertItemAsync(
            item: transaction,
            partitionKey: new PartitionKey("transactions"));
    }

    private Microsoft.Azure.Cosmos.Container GetContainer() => 
        cosmosClient.GetDatabase("TransferEasyDB").GetContainer("TransferEasyContainer");
}
