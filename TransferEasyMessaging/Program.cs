using TransferEasyMessaging;

var builder = Host.CreateApplicationBuilder(args);

builder.AddRabbitMQ("messagebus");

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
