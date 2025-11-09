using Microsoft.EntityFrameworkCore;
using MovieService.Data;
using MovieService.DomainLogic;
using MovieService.Helper;
using MovieService.Infrastructure.Repositories;
using MovieService.Infrastructure.Repositories.Interfaces;
using MovieService.Services;
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

RegisterRepository();
var app = builder.Build();
RegisterGrpcServicePublish();

void RegisterRepository()
{
    services.AddDbContext<MovieDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IGenreRepository, GenreRepository>();
    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IMovieRepository, MovieRepository>();
    services.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
    services.AddScoped<IMoviePersonRepository, MoviePersonRepository>();

    services.AddScoped<GetGenresLogic>();
    services.AddScoped<CreateGenreLogic>();
    services.AddScoped<UpdateGenreLogic>();
    services.AddScoped<DeleteGenreLogic>();

    services.AddScoped<GetPersonsLogic>();
    services.AddScoped<CreatePersonLogic>();
    services.AddScoped<UpdatePersonLogic>();
    services.AddScoped<DeletePersonLogic>();

    services.AddScoped<GetMoviesLogic>();
    services.AddScoped<CreateMovieLogic>();
    services.AddScoped<UpdateMovieLogic>();
    services.AddScoped<DeleteMovieLogic>();
}

void RegisterGrpcServicePublish()
{
    app.MapGrpcService<MovieGrpcServiceImpl>();
}

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
