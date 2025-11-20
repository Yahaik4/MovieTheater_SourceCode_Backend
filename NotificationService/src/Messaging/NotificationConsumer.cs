using Microsoft.AspNetCore.Connections;
using NotificationService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using src.Shared.Contracts.Messages;
using System.Text;
using System.Text.Json;

namespace CinemaService.Messaging
{
    public class NotificationConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IEmailService _emailService;

        public NotificationConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5673,
                UserName = "admin",
                Password = "123"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            string exchange = "notification.exchange";
            string queue = "notification.email.queue";
            string routingKey = "notification.email.otp";

            _channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);

            _channel.QueueDeclare(
                queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                queue,
                exchange,
                routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(args.Body.ToArray());
                    var message = JsonSerializer.Deserialize<SendOtpMessage>(body);

                    if (message == null)
                    {
                        Console.WriteLine("[MESSAGE RECEIVED] Null message!");
                        return;
                    }

                    Console.WriteLine($"[MESSAGE RECEIVED] Email:{message.Email}, Purpose:{message.Purpose}");

                    // Phân loại xử lý theo Purpose
                    switch (message.Purpose)
                    {
                        case "Register":
                        case "ForgotPassword":
                            if (!string.IsNullOrEmpty(message.Otp))
                                await _emailService.SendOtpAsync(message.Email, message.Otp);
                            break;
                        //case "BookingConfirmation":
                        //    SendBookingEmail(message.Email, message.Otp);
                        //    break;

                        //case "General":
                        //    if (!string.IsNullOrEmpty(message.Content))
                        //        SendNotificationEmail(message.Email, message.Content);
                        //    break;

                        default:
                            Console.WriteLine($"[UNKNOWN PURPOSE] Email:{message.Email}, Purpose:{message.Purpose}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }

}
