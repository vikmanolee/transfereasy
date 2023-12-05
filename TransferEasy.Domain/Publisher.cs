using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TransferEasy.Domain;

public interface IPublisher
{
    void Publish(TransactionEvent @event);
    void Publish(TransferCommand transfer);
    void Publish(DepositCommand deposit);
    void Publish(WithdrawCommand withdraw);
}

public class Publisher(IConnection connection) : IPublisher
{
    public void Publish(TransactionEvent @event)
    {
        Publish(JsonSerializer.Serialize(@event),
            "transaction_events",
             @event.Type.ToString());
    }

    public void Publish(TransferCommand transfer)
    {
        Publish(JsonSerializer.Serialize(transfer),
            "transaction_commands",
            $"transfer.{transfer.OriginAccount}.{transfer.DestinationAccount}");
    }

    public void Publish(DepositCommand deposit)
    {
        Publish(JsonSerializer.Serialize(deposit),
            "transaction_commands",
            $"deposit.{deposit.DestinationAccount}");
    }

    public void Publish(WithdrawCommand withdraw)
    {
        Publish(JsonSerializer.Serialize(withdraw),
            "transaction_commands",
            $"withdraw.{withdraw.OriginAccount}");
    }

    private void Publish(string message, string exchange, string routingKey)
    {
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: exchange,
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body);
    }
}


