using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TransferEasy.Domain;

public class EventHandler(IConnection connection, IProvideBalance balanceProvider) : BackgroundService
{
    public void HandleEvent(TransactionEvent transactionEvent)
    {
        balanceProvider.UpdateBalances(transactionEvent.Entries);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "transaction_event_q",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

        channel.QueueBind(queue: "transaction_event_q",
                  exchange: "transaction_events",
                  routingKey: string.Empty);

        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (object? model, BasicDeliverEventArgs ea) => ReceivedProcess(model, ea, channel);

        channel.BasicConsume(queue: "transaction_event_q",
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
        var transactionEvent = JsonSerializer.Deserialize<TransactionEvent>(message);

        if (transactionEvent != null)
            HandleEvent(transactionEvent);

        channel?.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    }
}

