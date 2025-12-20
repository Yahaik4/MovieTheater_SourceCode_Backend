using ApiGateway.ServiceConnector.MovieService;
using CinemaService.Data;
using CinemaService.DomainLogic;
using CinemaService.Helper;
using CinemaService.Infrastructure.Repositories;
using CinemaService.Infrastructure.Repositories.Implementations;
using CinemaService.Infrastructure.Repositories.Interfaces;
using CinemaService.ServiceConnector;
using CinemaService.Messaging;

using CinemaService.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RabbitMQ.Client;
using Shared.Utils;

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.EnableDynamicJson();

var dataSource = dataSourceBuilder.Build();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5673,
        UserName = "admin",
        Password = "123"
    };
    return factory.CreateConnection();
});

builder.Services.AddHostedService<ShowtimeSeatCleanupService>();
builder.Services.AddHostedService<BookingCleanupService>();
builder.Services.AddHostedService<PaymentStatusChangedConsumer>();

RegisterRepository();
var app = builder.Build();
RegisterGrpcServicePublish();

void RegisterRepository()
{
    services.AddDbContext<CinemaDbContext>(options =>
        options.UseNpgsql(dataSource)
    );

    // repository DJ
    services.AddScoped<ICinemaRepository, CinemaRepository>();
    services.AddScoped<IRoomRepository, RoomRepository>();
    services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
    services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
    services.AddScoped<ISeatRepository, SeatRepository>();
    services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
    services.AddScoped<IShowtimeSeatRepository, ShowtimeSeatRepository>();
    services.AddScoped<IBookingRepository, BookingRepository>();
    services.AddScoped<IFoodDrinkRepository, FoodDrinkRepository>();
    services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
    services.AddScoped<IHolidayRepository, HolidayRepository>();
    services.AddScoped<IPriceRuleRepository, PriceRuleRepository>();
    services.AddScoped<IPromotionRepository, PromotionRepository>();
    services.AddScoped<IBookingItemRepository, BookingItemRepository>();

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

    services.AddScoped<GetShowtimesByMovieLogic>();
    services.AddScoped<GetShowtimesByCinemaLogic>();
    services.AddScoped<GetShowtimeDetailsLogic>();
    services.AddScoped<CreateShowtimeLogic>();
    services.AddScoped<UpdateShowtimeLogic>();

    services.AddScoped<GetShowtimeSeatsLogic>();

    services.AddScoped<GetBookingLogic>();
    services.AddScoped<CreateBookingLogic>();
    services.AddScoped<UpdateBookingStatusLogic>();

    services.AddScoped<GetAllFoodDrinkLogic>();
    services.AddScoped<CreateFoodDrinkLogic>();
    services.AddScoped<UpdateFoodDrinkLogic>();
    services.AddScoped<DeleteFoodDrinkLogic>();

    services.AddScoped<CheckInBookingLogic>();
    services.AddScoped<GetBookingHistoryLogic>();

    services.AddScoped<GetCustomerTypesLogic>();
    services.AddScoped<CreateCustomerTypeLogic>();
    services.AddScoped<UpdateCustomerTypeLogic>();

    services.AddScoped<GetHolidaysLogic>();
    services.AddScoped<CreateHolidayLogic>();
    services.AddScoped<UpdateHolidayLogic>();

    services.AddScoped<SearchPromotionLogic>();
    services.AddScoped<GetPromotionsLogic>();
    services.AddScoped<CreatePromotionLogic>();
    services.AddScoped<UpdatePromotionLogic>();

    services.AddScoped<ProfileServiceConnector>();
    services.AddScoped<MovieServiceConnector>();
    services.AddHttpContextAccessor();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<CinemaGrpcServiceImpl>();
}

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
