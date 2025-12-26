using Microsoft.Extensions.DependencyInjection;
using NotificationService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Messages;
using System.Text;
using System.Text.Json;

namespace NotificationService.Messaging
{
    public class PaymentStatusChangedConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string Exchange = "payment.exchange";
        private const string Queue = "notification.payment.status.queue";
        private const string RoutingKey = "payment.status.changed";

        public PaymentStatusChangedConsumer(
            IConnection connection,
            IServiceScopeFactory scopeFactory)
        {
            _connection = connection;
            _scopeFactory = scopeFactory;

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Direct,
                durable: true);

            _channel.QueueDeclare(
                queue: Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                queue: Queue,
                exchange: Exchange,
                routingKey: RoutingKey);

            _channel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_, args) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var emailService =
                    scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());
                    Console.WriteLine($"[Notification] Received: {json}");

                    var message =
                        JsonSerializer.Deserialize<PaymentStatusChangedMessage>(json);

                    if (message == null)
                    {
                        _channel.BasicAck(args.DeliveryTag, false);
                        return;
                    }

                    // Chỉ gửi email khi thanh toán thành công
                    if (message.Status.Equals("paid", StringComparison.OrdinalIgnoreCase))
                    {
                        await emailService.SendEmailPaymentSuccess(
                            message.UserId,
                            message.BookingId);
                    }

                    _channel.BasicAck(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Notification] Error: " + ex.Message);

                    _channel.BasicNack(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            _channel.BasicConsume(
                queue: Queue,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
