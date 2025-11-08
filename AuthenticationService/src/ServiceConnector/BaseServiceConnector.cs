using Grpc.Net.Client;

namespace src.ServiceConnector
{
    public class ServiceConnectorConfigOption
    {
        public string Endpoint { get; set; } = string.Empty;
    }

    public class ServiceConnectorConfig
    {
        public ServiceConnectorConfigOption ProfileService { get; set; } = new ServiceConnectorConfigOption();
        public ServiceConnectorConfigOption OtpService { get; set; } = new ServiceConnectorConfigOption();
    }
    public class BaseServiceConnector
    {
        private readonly IConfiguration _configuration;

        protected BaseServiceConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ServiceConnectorConfig GetServiceConnectorConfig()
        {
            return new ServiceConnectorConfig
            {
                ProfileService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:ProfileService:Endpoint"] ?? string.Empty
                },
                OtpService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:OtpService:Endpoint"] ?? string.Empty
                }
            };
        }

        protected GrpcChannel GetGrpcChannel(string endpoint)
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return Grpc.Net.Client.GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions
            {
                HttpHandler = httpHandler,
                MaxReceiveMessageSize = 50 * 1024 * 1024,
                MaxSendMessageSize = 50 * 1024 * 1024
            });
        }

        protected GrpcChannel GetProfileServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().ProfileService.Endpoint);

        protected GrpcChannel GetOtpServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().OtpService.Endpoint); 
    }
}
