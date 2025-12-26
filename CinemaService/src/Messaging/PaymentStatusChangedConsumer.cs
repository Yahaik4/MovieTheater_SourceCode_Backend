using CinemaService.DataTransferObject.Parameter;
using CinemaService.DomainLogic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Messages;
using System.Text;
using System.Text.Json;

namespace CinemaService.Messaging
{
    public class PaymentStatusChangedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _exchange = "payment.exchange";
        private readonly string _queue = "cinema.payment.status.queue";
        private readonly string _routingKey = "payment.status.changed";

        public PaymentStatusChangedConsumer(IServiceScopeFactory scopeFactory, IConnection connection)
        {
            _scopeFactory = scopeFactory;
            _connection = connection;
            _channel = _connection.CreateModel();

            // Declare queue
            _channel.QueueDeclare(
                queue: _queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind queue vào exchange
            _channel.QueueBind(
                queue: _queue,
                exchange: _exchange,
                routingKey: _routingKey
            );

            _channel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, eventArgs) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var updateBookingLogic = scope.ServiceProvider.GetRequiredService<UpdateBookingStatusLogic>();

                try
                {
                    var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    Console.WriteLine($"[CinemaService] Received: {json}");

                    var message = JsonSerializer.Deserialize<PaymentStatusChangedMessage>(json);
                    if (message == null)
                    {
                        Console.WriteLine("[CinemaService] Invalid message format");
                        _channel.BasicAck(eventArgs.DeliveryTag, false);
                        return;
                    }

                    var updateParam = new UpdateBookingStatusParam
                    {
                        BookingId = message.BookingId,
                        Status = message.Status.ToLower()
                    };

                    await updateBookingLogic.Execute(updateParam);

                    Console.WriteLine($"[CinemaService] Booking {message.BookingId} updated -> {message.Status}");

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[CinemaService] Error: " + ex.Message);
                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume(_queue, autoAck: false, consumer);

            return Task.CompletedTask;
        }
    }
}
