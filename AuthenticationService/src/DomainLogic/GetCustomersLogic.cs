using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class GetCustomersLogic : IDomainLogic<GetUsersParam, Task<GetCustomersResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileServiceConnector;

        public GetCustomersLogic(
            IUserRepository userRepository,
            ProfileServiceConnector profileServiceConnector)
        {
            _userRepository = userRepository;
            _profileServiceConnector = profileServiceConnector;
        }

        public async Task<GetCustomersResultData> Execute(GetUsersParam param)
        {
            var users = await _userRepository.GetUsers(param?.UserId, UserRoleEnum.Customer);

            var resultItems = new List<CustomerWithProfileResultData>();

            foreach (var user in users)
            {
                var profile = await _profileServiceConnector.GetCustomerByUserId(user.Id);

                resultItems.Add(new CustomerWithProfileResultData
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    IsVerified = user.IsVerified,
                    FullName = profile?.FullName ?? string.Empty,
                    PhoneNumber = profile?.PhoneNumber ?? string.Empty,
                    DayOfBirth = profile?.DayOfBirth ?? string.Empty,
                    Gender = profile?.Gender ?? string.Empty,
                    Points = profile?.Points ?? 0
                });
            }

            return new GetCustomersResultData
            {
                Result = true,
                Message = "Success",
                StatusCode = StatusCodeEnum.Success,
                Data = resultItems
            };
        }
    }
}
