using RabbitMQ.Client;
using src.Shared.Contracts.Messages;
using System.Text;
using System.Text.Json;

namespace AuthenticationService.ServiceConnector
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName = "notification.exchange";

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5673,
                UserName = "admin",
                Password = "123"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Direct,
                durable: true
            );
        }

        public void PublishSendOtp(SendOtpMessage message)
        {
            var routingKey = "notification.email.otp";

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body
            );
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}