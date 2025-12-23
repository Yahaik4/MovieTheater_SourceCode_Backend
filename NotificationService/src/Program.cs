using CinemaService.Messaging;
using NotificationService.Helpers.cs;
using NotificationService.Messaging;
using NotificationService.ServiceConnector.AuthenticationService;
using NotificationService.ServiceConnector.CinemaService;
using NotificationService.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.AddSingleton<Mailer>();
builder.Services.AddHostedService<NotificationConsumer>();
builder.Services.AddHostedService<PaymentStatusChangedConsumer>();
builder.Services.AddScoped<IEmailService, SendOtpEmailService>();

builder.Services.AddScoped<AuthenticationServiceConnector>();
builder.Services.AddScoped<CinemaServiceConnector>();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
