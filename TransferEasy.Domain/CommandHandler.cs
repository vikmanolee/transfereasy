using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text.Json;

namespace TransferEasy.Domain;

public interface IHandleCommand
{
    void Handle(TransferCommand transfer);
    void Handle(DepositCommand deposit);
    void Handle(WithdrawCommand withdraw);
}

public class CommandHandler(IConnection connection, ILedgerService ledgerService, IPublisher publisher) : BackgroundService, IHandleCommand
{
    public void Handle(TransferCommand transfer)
    {
        var transaction = ledgerService.Transfer(transfer.Amount, transfer.OriginAccount, transfer.DestinationAccount);

        publisher.Publish(new UserTransferEvent(transaction));
    }

    public void Handle(DepositCommand deposit)
    {
        var transaction = ledgerService.Deposit(deposit.Amount, deposit.DestinationAccount);

        publisher.Publish(new DepositEvent(transaction));
    }

    public void Handle(WithdrawCommand withdraw)
    {
        var transaction = ledgerService.Withdraw(withdraw.Amount, withdraw.OriginAccount);

        publisher.Publish(new WithdrawalEvent(transaction));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "transaction_command_q",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

        channel.QueueBind(queue: "transaction_command_q",
                  exchange: "transaction_commands",
                  routingKey: string.Empty);

        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (object? model, BasicDeliverEventArgs ea) => ReceivedProcess(model, ea, channel);

        channel.BasicConsume(queue: "transaction_command_q",
                             autoAck: false,
                             consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {

        }
    }

    private void ReceivedProcess(object? model, BasicDeliverEventArgs ea, IModel? channel)
    {
        byte[] body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        var rk = ea.RoutingKey;
        var type = rk.Split(".").First();

        switch (type)
        {
            case "transfer":
                var transferCommand = JsonSerializer.Deserialize<TransferCommand>(message);
                if (transferCommand != null) Handle(transferCommand);
                break;
            case "deposit":
                var depositCommand = JsonSerializer.Deserialize<DepositCommand>(message);
                if (depositCommand != null) Handle(depositCommand);
                break;
            case "withdraw":
                var withdrawCommand = JsonSerializer.Deserialize<WithdrawCommand>(message);
                if (withdrawCommand != null) Handle(withdrawCommand);
                break;
            default:
                break;
        }

        channel?.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    }
}
