using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class GetStaffsLogic : IDomainLogic<GetUsersParam, Task<GetStaffsResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileServiceConnector;

        public GetStaffsLogic(
            IUserRepository userRepository,
            ProfileServiceConnector profileServiceConnector)
        {
            _userRepository = userRepository;
            _profileServiceConnector = profileServiceConnector;
        }

        public async Task<GetStaffsResultData> Execute(GetUsersParam param)
        {
            var users = await _userRepository.GetUsers(param?.UserId, UserRoleEnum.Staff);

            var resultItems = new List<StaffWithProfileResultData>();

            foreach (var user in users)
            {
                var profile = await _profileServiceConnector.GetStaffByUserId(user.Id);

                resultItems.Add(new StaffWithProfileResultData
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    IsVerified = user.IsVerified,
                    FullName = profile?.FullName ?? string.Empty,
                    PhoneNumber = profile?.PhoneNumber ?? string.Empty,
                    DayOfBirth = profile?.DayOfBirth ?? string.Empty,
                    Gender = profile?.Gender ?? string.Empty,
                    CinemaId = profile?.CinemaId ?? Guid.Empty,
                    Position = profile?.Position ?? string.Empty,
                    Salary = profile?.Salary ?? 0
                });
            }

            return new GetStaffsResultData
            {
                Result = true,
                Message = "Success",
                StatusCode = StatusCodeEnum.Success,
                Data = resultItems
            };
        }
    }
}
