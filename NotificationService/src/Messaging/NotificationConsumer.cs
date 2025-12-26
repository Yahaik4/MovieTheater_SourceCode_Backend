using Microsoft.Extensions.DependencyInjection;
using NotificationService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Messages;
using System.Text;
using System.Text.Json;

namespace CinemaService.Messaging
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string Exchange = "notification.exchange";
        private const string Queue = "notification.email.queue";
        private const string RoutingKey = "notification.email.otp";

        public NotificationConsumer(
            IConnection connection,
            IServiceScopeFactory scopeFactory) // ✅ ĐÚNG
        {
            _connection = connection;
            _scopeFactory = scopeFactory;

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(Exchange, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(Queue, Exchange, RoutingKey);
            _channel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_, args) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider
                                        .GetRequiredService<IEmailService>();

                try
                {
                    var body = Encoding.UTF8.GetString(args.Body.ToArray());
                    var message = JsonSerializer.Deserialize<SendOtpMessage>(body);

                    if (message == null)
                    {
                        _channel.BasicAck(args.DeliveryTag, false);
                        return;
                    }

                    var purpose = message.Purpose?.ToLowerInvariant();

                    switch (purpose)
                    {
                        case "register":
                        case "forgotpassword":
                        case "reset_password":
                            if (!string.IsNullOrEmpty(message.Otp))
                                await emailService.SendOtpAsync(
                                    message.Email,
                                    message.Otp);
                            break;
                    }

                    _channel.BasicAck(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");
                    _channel.BasicNack(args.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(Queue, autoAck: false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
