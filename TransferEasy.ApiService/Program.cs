using TransferEasy.ApiService;
using TransferEasy.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AccountContext>("TransferEasyDB");

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

TransferEasyEndpoints.Map(app);

app.MapDefaultEndpoints();

app.Run();
