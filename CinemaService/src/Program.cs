using Microsoft.EntityFrameworkCore;
using Shared.Utils;
using src.Data;
using src.DomainLogic;
using src.Helper;
using src.Infrastructure.Repositories;
using src.Infrastructure.Repositories.Interfaces;
using src.Services;

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
    services.AddDbContext<CinemaDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // repository DJ
    services.AddScoped<IRoomRepository, RoomRepository>();
    services.AddScoped<ICinemaRepository, CinemaRepository>();

    // logic DJ
    services.AddScoped<GetAllCinemaLogic>();
    services.AddScoped<CreateCinemaLogic>();
    services.AddScoped<CreateRoomsLogic>();

    //services.AddScoped<ProfileServiceConnector>();
    //services.AddHttpContextAccessor();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<CinemaGrpcServiceImpl>();
}

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
