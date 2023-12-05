namespace TransferEasyMessaging;

using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using TransferEasy.Domain;

public class Worker : BackgroundService
{
    readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _bus.Publish(new Account() { Name = "Foo"}, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
