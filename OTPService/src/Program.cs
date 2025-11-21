using Shared.Utils;
using OTPService.Infrastructure.Repositories.Interfaces;
using OTPService.Data;
using OTPService.DomainLogic;
using OTPService.Services;
using OTPService.Helper;
using Microsoft.EntityFrameworkCore;
using OTPService.Infrastructure.Repositories;

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

RegisterRepository();
var app = builder.Build();
RegisterGrpcServicePublish();

void RegisterRepository()
{
    services.AddDbContext<OTPDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IOTPRepository, OTPRepository>();

    services.AddScoped<CreateOTPLogic>();
    services.AddScoped<VerifyOTPLogic>();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<OTPGrpcServiceImpl>();
}

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
