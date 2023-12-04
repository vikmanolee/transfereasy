var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var db = builder.AddPostgresContainer("postgres", password: "s!llyP@ss").AddDatabase("TransferEasyDB");

var apiservice = builder.AddProject<Projects.TransferEasy_ApiService>("apiservice")
    .WithReference(db);

builder.AddProject<Projects.TransferEasy_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.Build().Run();
