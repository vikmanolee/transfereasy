
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var db = builder.AddPostgresContainer("postgres", password: "s!llyP@ss").AddDatabase("TransferEasyDB");

var cosmosdb = builder.AddAzureCosmosDB("cosmosdb");

var messaging = builder.AddRabbitMQContainer("messagebus");

var apiservice = builder.AddProject<Projects.TransferEasy_ApiService>("apiservice")
    .WithReference(db)
    .WithReference(cache)
    .WithReference(cosmosdb)
    .WithReference(messaging);

builder.AddProject<Projects.TransferEasy_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.Build().Run();
