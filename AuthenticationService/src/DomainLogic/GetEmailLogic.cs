using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class GetEmailLogic : IDomainLogic<GetEmailParam, Task<GetEmailResultData>>
    {
        private readonly IUserRepository _userRepository;

        public GetEmailLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetEmailResultData> Execute(GetEmailParam param)
        {
            var users = await _userRepository.GetUserById(param.UserId);

            if (users == null) {
                throw new NotFoundException("User Not Found");
            }

            return new GetEmailResultData
            {
                Result = true,
                Message = "Get Email Success",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetEmailDataResult
                {
                    Email = users.Email,
                }
            };
        }
    }
}
