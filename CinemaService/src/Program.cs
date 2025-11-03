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
    services.AddScoped<ICinemaRepository, CinemaRepository>();
    services.AddScoped<IRoomRepository, RoomRepository>();
    services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
    services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
    services.AddScoped<ISeatRepository, SeatRepository>();

    // logic DJ
    services.AddScoped<GetAllCinemaLogic>();
    services.AddScoped<CreateCinemaLogic>();
    services.AddScoped<UpdateCinemaLogic>();
    services.AddScoped<DeleteCinemaLogic>();

    services.AddScoped<GetAllRoomLogic>();
    services.AddScoped<CreateRoomsLogic>();
    services.AddScoped<UpdateRoomLogic>();
    services.AddScoped<DeleteRoomLogic>();

    services.AddScoped<GetAllRoomTypeLogic>();
    services.AddScoped<CreateRoomTypeLogic>();
    services.AddScoped<UpdateRoomTypeLogic>();
    services.AddScoped<DeleteRoomTypeLogic>();

    services.AddScoped<GetAllSeatLogic>();
    services.AddScoped<UpdateSeatLogic>();

    services.AddScoped<GetAllSeatTypeLogic>();
    services.AddScoped<CreateSeatTypeLogic>();
    services.AddScoped<UpdateSeatTypeLogic>();
    services.AddScoped<DeleteSeatTypeLogic>();

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
