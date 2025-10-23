using AuthenticationGrpc;
using CinemaGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;

namespace src.ServiceConnector.CinemaService
{
    public class CinemaServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public CinemaServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<GetAllCinemasGrpcReplyDTO> GetAllCinemas(Guid? id, string? name,string? city, string? status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllCinemasGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            if (!string.IsNullOrWhiteSpace(city))
                request.City = city;

            if (!string.IsNullOrWhiteSpace(status))
                request.Status = status;

            return await client.GetAllCinemasAsync(request);
        }

        public async Task<CreateCinemaGrpcReplyDTO> CreateCinema(string name, string address, string city, string phoneNumber, string email, string open_time, string close_time, string status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateCinemaGrpcRequestDTO
            {
                Name = name,
                Address = address,
                City = city,
                PhoneNumber = phoneNumber,
                Email = email,
                OpenTime = open_time,
                CloseTime = close_time,
                Status = status
            };

            return await client.CreateCinemaAsync(request);
        }
    }
}
