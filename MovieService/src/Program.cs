using AutoMapper.Internal;
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
//builder.Services.AddAutoMapper(cfg =>
//{
//    cfg.AllowNullCollections = true;
//    cfg.AllowNullDestinationValues = true;

//    // ? Cách làm chu?n cho AutoMapper 12+
//    cfg.Internal().ForAllMaps((typeMap, map) =>
//    {
//        map.ForAllMembers(opt =>
//        {
//            if (opt.DestinationMember.GetMemberType() == typeof(string))
//            {
//                opt.NullSubstitute(string.Empty);
//            }
//        });
//    });
//}, typeof(AutoMap));

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
    services.AddDbContext<MovieDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IGenreRepository, GenreRepository>();
    services.AddScoped<IPersonRepository, PersonRepository>();

    services.AddScoped<GetGenresLogic>();
    services.AddScoped<CreateGenreLogic>();
    services.AddScoped<UpdateGenreLogic>();
    services.AddScoped<DeleteGenreLogic>();

    services.AddScoped<GetPersonsLogic>();
    services.AddScoped<CreatePersonLogic>();
    services.AddScoped<UpdatePersonLogic>();
    services.AddScoped<DeletePersonLogic>();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<MovieGrpcServiceImpl>();
}

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
