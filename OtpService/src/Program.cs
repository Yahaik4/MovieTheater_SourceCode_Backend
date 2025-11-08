using Microsoft.EntityFrameworkCore;
using Shared.Utils;
using src.Infrastructure.Repositories;
using src.Infrastructure.Repositories.Interfaces;
using src.DomainLogic;
using src.Helper;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Current Environment: " + builder.Environment.EnvironmentName);
Console.WriteLine("Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection"));

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMap>());


// Add gRPC services
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<BaseExceptionInterceptor>();
});

// ===================================
// Đăng ký repository, logic, helper
// ===================================
RegisterRepository();

// Build app
var app = builder.Build();

// Đăng ký gRPC endpoint
RegisterGrpcServicePublish();

// ===================================
// Các hàm con hỗ trợ cấu trúc gọn gàng
// ===================================
void RegisterRepository()
{
    // Đăng ký DbContext
    builder.Services.AddDbContext<OtpDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Repository DI
    builder.Services.AddScoped<IOtpRepository, OtpRepository>();

    // Helper / Services
    builder.Services.AddScoped<Email>();

    // Domain Logic DI
    builder.Services.AddScoped<SendOtpLogic>();
    builder.Services.AddScoped<ValidateOtpLogic>();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<OtpGrpcServiceImpl>();
}

// ===================================
// HTTP info (optional, như CinemaService)
// ===================================
app.MapGet("/", () =>
    "Communication with gRPC endpoints must be made through a gRPC client. " +
    "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// Run app
app.Run();
