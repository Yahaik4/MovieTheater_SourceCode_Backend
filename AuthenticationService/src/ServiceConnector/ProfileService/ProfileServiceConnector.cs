using AuthenticationGrpc;
using AuthenticationService.ServiceConnector;
using Grpc.Core;
using ProfileGrpc;
using AuthenticationService.DataTransferObject.ResultData;

namespace AuthenticationService.ServiceConnector.ProfileService
{
    public class ProfileServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public ProfileServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }


        public async Task<CreateProfileGrpcReplyDTO> CreateProfile(
            string fullname,
            string role,
            string userId,
            string? phoneNumber = null,
            string? dayOfBirth = null,
            string? gender = null,
            string? cinemaId = null,
            string? position = null,
            string? salary = null
            )
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new CreateProfileGrpcRequestDTO
            {
                FullName = fullname,
                Role = role,
                UserId = userId,
                PhoneNumber = phoneNumber ?? "",
                DayOfBirth = dayOfBirth ?? "",
                Gender = gender ?? "",
                CinemaId = cinemaId ?? "",
                Position = position ?? "",
                Salary = salary ?? ""
            };

            return await client.CreateProfileAsync(request);
        }
        
        public async Task<GetStaffResultData?> GetStaffByUserId(Guid userId)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new GetStaffByUserIdRequest
            {
                UserId = userId.ToString()
            };

            var reply = await client.GetStaffByUserIdAsync(request);

            if (!reply.Found)
                return null;

            return new GetStaffResultData
            {
                UserId = Guid.Parse(reply.UserId),
                CinemaId = Guid.Parse(reply.CinemaId),
                Position = reply.Position,
                FullName = reply.FullName,
                PhoneNumber = reply.PhoneNumber,
                DayOfBirth = reply.DayOfBirth,
                Gender = reply.Gender,
                Salary = decimal.TryParse(reply.Salary, out var salary) ? salary : 0
            };
        }

        public async Task<GetCustomerProfileResultData?> GetCustomerByUserId(Guid userId)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new GetCustomerByUserIdRequest
            {
                UserId = userId.ToString()
            };

            var reply = await client.GetCustomerByUserIdAsync(request);

            if (!reply.Found)
                return null;

            return new GetCustomerProfileResultData
            {
                UserId = Guid.Parse(reply.UserId),
                FullName = reply.FullName,
                PhoneNumber = reply.PhoneNumber,
                DayOfBirth = reply.DayOfBirth,
                Gender = reply.Gender,
                Points = reply.Points
            };
        }

        public async Task DeleteCustomerByUserId(Guid userId)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new DeleteProfileByUserIdRequest
            {
                UserId = userId.ToString()
            };

            await client.DeleteCustomerByUserIdAsync(request);
        }

        public async Task DeleteStaffByUserId(Guid userId)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new DeleteProfileByUserIdRequest
            {
                UserId = userId.ToString()
            };

            await client.DeleteStaffByUserIdAsync(request);
        }

        public async Task<UpdateProfileGrpcReplyDTO> UpdateCustomerByUserId(
            Guid userId,
            string? fullName,
            string? phoneNumber,
            string? dayOfBirth,
            string? gender,
            int? points)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new UpdateCustomerByUserIdRequest
            {
                UserId = userId.ToString(),
                FullName = fullName ?? string.Empty,
                PhoneNumber = phoneNumber ?? string.Empty,
                DayOfBirth = dayOfBirth ?? string.Empty,
                Gender = gender ?? string.Empty,
                Points = points?.ToString() ?? string.Empty
            };

            return await client.UpdateCustomerByUserIdAsync(request);
        }

        public async Task<UpdateProfileGrpcReplyDTO> UpdateStaffByUserId(
            Guid userId,
            string? fullName,
            string? phoneNumber,
            string? dayOfBirth,
            string? gender,
            Guid? cinemaId,
            string? position,
            decimal? salary)
        {
            using var channel = GetProfileServiceChannel();
            var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);

            var request = new UpdateStaffByUserIdRequest
            {
                UserId = userId.ToString(),
                FullName = fullName ?? string.Empty,
                PhoneNumber = phoneNumber ?? string.Empty,
                DayOfBirth = dayOfBirth ?? string.Empty,
                Gender = gender ?? string.Empty,
                CinemaId = cinemaId?.ToString() ?? string.Empty,
                Position = position ?? string.Empty,
                Salary = salary?.ToString() ?? string.Empty
            };

            return await client.UpdateStaffByUserIdAsync(request);
        }
    }
}
