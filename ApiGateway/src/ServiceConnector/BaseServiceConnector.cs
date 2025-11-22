using Grpc.Net.Client;

namespace ApiGateway.ServiceConnector
{
    public class ServiceConnectorConfigOption
    {
        public string Endpoint { get; set; } = string.Empty;
    }

    public class ServiceConnectorConfig
    {
        public ServiceConnectorConfigOption AuthenticationService { get; set; } = new ServiceConnectorConfigOption();
        public ServiceConnectorConfigOption CinemaService { get; set; } = new ServiceConnectorConfigOption();
        public ServiceConnectorConfigOption MovieService { get; set; } = new ServiceConnectorConfigOption();
        public ServiceConnectorConfigOption OTPService { get; set; } = new ServiceConnectorConfigOption();
        public ServiceConnectorConfigOption PaymentService { get; set; } = new ServiceConnectorConfigOption();


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
                AuthenticationService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:AuthService:Endpoint"] ?? string.Empty
                },
                CinemaService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:CinemaService:Endpoint"] ?? string.Empty
                },
                MovieService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:MovieService:Endpoint"] ?? string.Empty
                },
                OTPService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:OTPService:Endpoint"] ?? string.Empty
                },
                PaymentService = new ServiceConnectorConfigOption
                {
                    Endpoint = _configuration["ServiceConnector:PaymentService:Endpoint"] ?? string.Empty
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

        protected GrpcChannel GetAuthenticationServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().AuthenticationService.Endpoint);

        protected GrpcChannel GetCinemaServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().CinemaService.Endpoint);

        protected GrpcChannel GetMovieServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().MovieService.Endpoint);

        protected GrpcChannel GetOTPServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().OTPService.Endpoint);

        protected GrpcChannel GetPaymentServiceChannel()
            => GetGrpcChannel(GetServiceConnectorConfig().PaymentService.Endpoint);
    }
}
