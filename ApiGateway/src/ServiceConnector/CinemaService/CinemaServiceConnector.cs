using AuthenticationGrpc;
using CinemaGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using src.Helper;
using System;
using System.Net;
using System.Xml.Linq;

namespace src.ServiceConnector.CinemaService
{
    public class CinemaServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;
        private readonly ICurrentUserService _currentUserService;

        public CinemaServiceConnector(IConfiguration configuration, ICurrentUserService currentUserService) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
            _currentUserService = currentUserService;
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
                Status = status,
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateCinemaAsync(request);
        }

        public async Task<UpdateCinemaGrpcReplyDTO> UpdateCinema(Guid id, string name, string address, string city, string phoneNumber, string email, string open_time, string close_time, string status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateCinemaGrpcRequestDTO
            {
                Id = id.ToString(),
                Name = name,
                Address = address,
                City = city,
                PhoneNumber = phoneNumber,
                Email = email,
                OpenTime = open_time,
                CloseTime = close_time,
                Status = status
            };

            return await client.UpdateCinemaAsync(request);
        }

        public async Task<DeleteCinemaGrpcReplyDTO> DeleteCinema(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteCinemaGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteCinemaAsync(request);
        }

        public async Task<GetAllRoomTypesGrpcReplyDTO> GetAllRoomTypes(Guid? id, string? type, decimal? basePrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllRoomTypesGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(type))
                request.Type = type;

            if (basePrice.HasValue)
                request.BasePrice = basePrice.Value.ToString();

            return await client.GetAllRoomTypesAsync(request);
        }

        public async Task<CreateRoomTypeGrpcReplyDTO> CreateRoomType(string type, decimal basePrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateRoomTypeGrpcRequestDTO
            {
                Type = type,
                BasePrice = basePrice.ToString(),
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateRoomTypeAsync(request);
        }

        public async Task<UpdateRoomTypeGrpcReplyDTO> UpdateRoomType(Guid id, string type, decimal basePrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateRoomTypeGrpcRequestDTO
            {
                Id = id.ToString(),
                Type = type,
                BasePrice = basePrice.ToString(),
            };

            return await client.UpdateRoomTypeAsync(request);
        }

        public async Task<DeleteRoomTypeGrpcReplyDTO> DeleteRoomType(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteRoomTypeGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteRoomTypeAsync(request);
        }

        public async Task<GetAllSeatTypesGrpcReplyDTO> GetAllSeatTypes(Guid? id, string? type, decimal? extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllSeatTypesGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(type))
                request.Type = type;

            if (extraPrice.HasValue)
                request.ExtraPrice = extraPrice.Value.ToString();

            return await client.GetAllSeatTypesAsync(request);
        }

        public async Task<CreateSeatTypeGrpcReplyDTO> CreateSeatType(string type, decimal extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateSeatTypeGrpcRequestDTO
            {
                Type = type,
                ExtraPrice = extraPrice.ToString(),
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateSeatTypeAsync(request);
        }

        public async Task<UpdateSeatTypeGrpcReplyDTO> UpdateSeatType(Guid id, string type, decimal extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateSeatTypeGrpcRequestDTO
            {
                Id = id.ToString(),
                Type = type,
                ExtraPrice = extraPrice.ToString(),
            };

            return await client.UpdateSeatTypeAsync(request);
        }

        public async Task<DeleteSeatTypeGrpcReplyDTO> DeleteSeatType(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteSeatTypeGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteSeatTypeAsync(request);
        }
    }
}
