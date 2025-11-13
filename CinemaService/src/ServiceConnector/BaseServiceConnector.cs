using Grpc.Net.Client;

namespace ApiGateway.ServiceConnector
{
    public class ServiceConnectorConfigOption
    {
        public string Endpoint { get; set; } = string.Empty;
    }

    public class ServiceConnectorConfig
    {
        public ServiceConnectorConfigOption MovieService { get; set; } = new ServiceConnectorConfigOption();

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
                MovieService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:MovieService:Endpoint"] ?? string.Empty
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

            return GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions
            {
                HttpHandler = httpHandler,
                MaxReceiveMessageSize = 50 * 1024 * 1024,
                MaxSendMessageSize = 50 * 1024 * 1024
            });
        }

        protected GrpcChannel GetMovieServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().MovieService.Endpoint);

    }
}
