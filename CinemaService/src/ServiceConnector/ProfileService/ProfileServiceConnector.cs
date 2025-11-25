using Grpc.Net.Client;
using ProfileGrpc;

namespace CinemaService.ServiceConnector
{
    public class ProfileServiceConnector
    {
        private readonly IConfiguration _config;

        public ProfileServiceConnector(IConfiguration config)
        {
            _config = config;
        }

        private GrpcChannel GetChannel()
        {
            var url = _config["ServiceConnector:ProfileService:Endpoint"];
            return GrpcChannel.ForAddress(url, new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
        }

        public async Task<GetStaffByUserIdReply> GetStaffs(Guid? staffUserId, Guid? cinemaId)
        {
            using var channel = GetChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new GetStaffsRequest();

            if (staffUserId.HasValue)
                request.UserId = staffUserId.Value.ToString();

            if (cinemaId.HasValue)
                request.CinemaId = cinemaId.Value.ToString();

            return await client.GetStaffsAsync(request);
        }
    }
}
