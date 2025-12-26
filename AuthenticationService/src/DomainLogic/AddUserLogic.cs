using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.EF.Models;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.DomainLogic
{
    public class AddUserLogic : IDomainLogic<AddUserParam, Task<RegisterResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileServiceConnector;

        public AddUserLogic(IUserRepository userRepository, ProfileServiceConnector profileServiceConnector)
        {
            _userRepository = userRepository;
            _profileServiceConnector = profileServiceConnector;
        }

        public async Task<RegisterResultData> Execute(AddUserParam param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));


            var existed = await _userRepository.GetUserByEmailIncludingUnverified(param.Email);

            if (existed != null && existed.IsDeleted == false)
                throw new ValidationException("Email is already registered.");


            if (param.Role == StaffPositionEnum.Staff)
                ValidateStaffParam(param);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = param.Email,
                Password = HashPassword(param.Password),
                Role = param.Role,
                IsVerified = true
            };

            await _userRepository.CreateUser(newUser);

            try
            {

                string? dobStr = param.DayOfBirth?.ToString("yyyy-MM-dd");

                var profile = await _profileServiceConnector.CreateProfile(
                    fullname: param.FullName,
                    role: newUser.Role,
                    userId: newUser.Id.ToString(),
                    phoneNumber: param.PhoneNumber,
                    dayOfBirth: dobStr,
                    gender: param.Gender,
                    cinemaId: param.CinemaId?.ToString(),
                    position: param.Position,
                    salary: param.Salary?.ToString() ?? string.Empty
                );

                if (!profile.Result)
                {
                    await _userRepository.RemoveUser(newUser);
                    throw new Exception("Failed to create profile.");
                }
            }
            catch (Exception ex)
            {
                await _userRepository.RemoveUser(newUser);
                return new RegisterResultData
                {
                    Result = false,
                    Message = ex.Message,
                    StatusCode = StatusCodeEnum.InternalServerError
                };
            }

            return new RegisterResultData
            {
                Result = true,
                Message = "Add user successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new RegisterDataResult
                {
                    UserId = newUser.Id
                }
            };
        }

        private void ValidateStaffParam(AddUserParam param)
        {
            var missingFields = new List<string>();

            if (string.IsNullOrWhiteSpace(param.FullName))
                missingFields.Add(nameof(param.FullName));
            if (string.IsNullOrWhiteSpace(param.Email))
                missingFields.Add(nameof(param.Email));
            if (string.IsNullOrWhiteSpace(param.Password))
                missingFields.Add(nameof(param.Password));

            if (string.IsNullOrWhiteSpace(param.PhoneNumber))
                missingFields.Add(nameof(param.PhoneNumber));
            if (!param.DayOfBirth.HasValue)
                missingFields.Add(nameof(param.DayOfBirth));
            if (string.IsNullOrWhiteSpace(param.Gender))
                missingFields.Add(nameof(param.Gender));

            if (!param.CinemaId.HasValue)
                missingFields.Add(nameof(param.CinemaId));
            if (string.IsNullOrWhiteSpace(param.Position))
                missingFields.Add(nameof(param.Position));

            if (missingFields.Any())
                throw new ValidationException(
                    "Missing required fields for staff: " + string.Join(", ", missingFields));


            var validPositions = new[]
            {
                StaffPositionEnum.Staff,         
                StaffPositionEnum.CinemaManager,     
                StaffPositionEnum.OperationsManager  
            };

            if (!validPositions.Contains(param.Position!))
            {
                throw new ValidationException(
                    $"Invalid staff position '{param.Position}'. " +
                    $"Allowed: {string.Join(", ", validPositions)}");
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
