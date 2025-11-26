using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DomainLogic;
using PaymentService.Helper;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.Repositories.Interfaces;
using PaymentService.Providers;
using PaymentService.ServiceConnector;
using PaymentService.ServiceConnector.CinemaService;
using PaymentService.Services;
using Shared.Utils;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Current Environment: " + builder.Environment.EnvironmentName);
Console.WriteLine("Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddAutoMapper(typeof(AutoMap));

// Add services to the container.
builder.Services.AddGrpc();
var services = builder.Services;
{
    services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = true;
        options.Interceptors.Add<BaseExceptionInterceptor>();
    });
}
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

RegisterRepository();
var app = builder.Build();
RegisterGrpcServicePublish();

void RegisterRepository()
{
    services.AddDbContext<PaymentDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ITransactionRepository, TransactionRepository>();

    services.AddScoped<StripePaymentProvider>();
    services.AddScoped<VnPayPaymentProvider>();
    services.AddScoped<MomoPaymentProvider>();

    services.AddScoped<PaymentProviderFactory>();
    services.AddScoped<RabbitMqPublisher>();

    services.AddScoped<CreateTransactionLogic>();
    services.AddScoped<HandleVnpayCallbackLogic>();

    services.AddScoped<CinemaServiceConnector>();
    services.AddHttpContextAccessor();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<PaymentGrpcServiceImpl>();
}

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
